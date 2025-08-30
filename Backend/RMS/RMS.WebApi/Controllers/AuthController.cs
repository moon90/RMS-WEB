using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Application.Helpers;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.Dtos.UserDTOs.InputDTOs;
using RMS.Domain.DTOs;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;
using RMS.Domain.Models.BaseModels;

namespace RMS.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly IValidator<UserCreateDto> _userCreateValidator;

        public AuthController(IUserService userService, ITokenService tokenService, IMapper mapper, IValidator<UserCreateDto> userCreateValidator, IEmailSender emailSender)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mapper = mapper;
            _userCreateValidator = userCreateValidator;
            _emailSender = emailSender;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                // Authenticate the user
                var authResult = await _userService.AuthenticateAsync(loginDto.UserName, loginDto.Password);
                if (!authResult.IsSuccess)
                    return Unauthorized(authResult);

                var user = authResult.Data;

                // Check role permissions
                var rolePermissionsResult = await _userService.GetRolePermissionsAsync(user.UserID);
                if (!rolePermissionsResult.IsSuccess || rolePermissionsResult.Data == null || !rolePermissionsResult.Data.Any())
                {
                    return Unauthorized(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "User has no role permissions.",
                        Code = "AUTH_NO_ROLE_PERMISSIONS"
                    });
                }

                // Check menu permissions
                var menuPermissionsResult = await _userService.GetMenuPermissionsAsync(user.UserID);
                if (!menuPermissionsResult.IsSuccess || menuPermissionsResult.Data == null || !menuPermissionsResult.Data.Any())
                {
                    return Unauthorized(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "User has no menu permissions.",
                        Code = "AUTH_NO_MENU_PERMISSIONS"
                    });
                }

                // Generate tokens
                var accessToken = _tokenService.GenerateToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken();

                // Save the refresh token in the database
                var refreshUpdateResult = await _userService.UpdateRefreshTokenAsync(user.UserID, refreshToken, DateTime.UtcNow.AddDays(7));
                if (!refreshUpdateResult.IsSuccess)
                {
                    return StatusCode(500, refreshUpdateResult);
                }

                // Prepare response DTO
                var responseData = new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    User = user,
                    RolePermissions = rolePermissionsResult.Data,
                    MenuPermissions = menuPermissionsResult.Data
                };

                return Ok(new ResponseDto<AuthResponseDto>
                {
                    IsSuccess = true,
                    Message = "Login successful",
                    Code = "200",
                    Data = responseData
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDto userCreateDto)
        {
            try
            {
                var result = await _userService.CreateUserAsync(userCreateDto);

                if (!result.IsSuccess)
                    return BadRequest(result); // includes validation errors via `Details`

                return CreatedAtAction(nameof(GetUserById), new { id = result.Data.UserID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while registering the user.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<UserDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the user.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var userResult = await _userService.GetUserByRefreshTokenAsync(refreshTokenDto.RefreshToken);
                if (!userResult.IsSuccess || userResult.Data == null)
                {
                    return Unauthorized(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Invalid refresh token.",
                        Code = "INVALID_REFRESH_TOKEN"
                    });
                }

                var user = userResult.Data;

                var validationResult = _tokenService.ValidateRefreshToken(refreshTokenDto.RefreshToken, user.RefreshTokenExpiry);
                if (!validationResult.IsValid)
                {
                    return Unauthorized(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = validationResult.Message,
                        Code = "REFRESH_TOKEN_EXPIRED"
                    });
                }

                var newAccessToken = _tokenService.GenerateToken(user);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                var updateResult = await _userService.UpdateRefreshTokenAsync(user.UserID, newRefreshToken, DateTime.UtcNow.AddDays(7));
                if (!updateResult.IsSuccess)
                {
                    return StatusCode(500, updateResult);
                }

                var authResponse = new AuthResponseDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    User = user
                };

                return Ok(new ResponseDto<AuthResponseDto>
                {
                    IsSuccess = true,
                    Message = "Token refreshed successfully.",
                    Code = "200",
                    Data = authResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred while refreshing the token.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto logoutDto)
        {
            try
            {
                int userId = logoutDto.UserId;
                var result = await _userService.UpdateRefreshTokenAsync(userId, null, null);

                if (!result.IsSuccess)
                    return BadRequest(result); // or NotFound(result) if USER_NOT_FOUND

                return Ok(new ResponseDto<string>
                {
                    IsSuccess = true,
                    Message = "Logged out successfully.",
                    Code = "200",
                    Data = $"UserId:{userId}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred while logging out.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordDto forgetPasswordDto)
        {
            try
            {
                var userResult = await _userService.GetUserByEmailAsync(forgetPasswordDto.Email);
                if (!userResult.IsSuccess || userResult.Data == null)
                {
                    return NotFound(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Email not registered.",
                        Code = "EMAIL_NOT_FOUND"
                    });
                }

                var user = userResult.Data;
                var resetToken = Guid.NewGuid().ToString();
                var expiryTime = DateTime.UtcNow.AddMinutes(15);

                var tokenResult = await _userService.SetPasswordResetTokenAsync(user.UserID, resetToken, expiryTime);
                if (!tokenResult.IsSuccess)
                {
                    return StatusCode(500, tokenResult);
                }

                var resetLink = $"https://rms.enlightenedpharma.net/reset-password?token={resetToken}&email={forgetPasswordDto.Email}";

                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "PasswordResetTemplate.html");
                var body = await System.IO.File.ReadAllTextAsync(templatePath);
                body = body.Replace("{{ResetLink}}", resetLink);

                await _emailSender.SendEmailAsync(user.Email, "Password Reset Request", body);

                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Password reset email sent.",
                    Code = "200",
                    Data = new { Email = user.Email }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while processing the password reset request.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }


        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var userResult = await _userService.GetUserByEmailAsync(resetPasswordDto.Email);
                if (!userResult.IsSuccess || userResult.Data == null)
                {
                    return NotFound(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Invalid email.",
                        Code = "INVALID_EMAIL"
                    });
                }

                var user = userResult.Data;

                if (string.IsNullOrEmpty(user.PasswordResetToken) || user.PasswordResetToken != resetPasswordDto.ResetToken)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Invalid reset token.",
                        Code = "INVALID_RESET_TOKEN"
                    });
                }

                if (user.PasswordResetTokenExpiry == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Reset token has expired.",
                        Code = "RESET_TOKEN_EXPIRED"
                    });
                }

                var (passwordHash, passwordSalt) = PasswordHelper.CreatePasswordHash(resetPasswordDto.NewPassword);

                var updateResult = await _userService.UpdateUserPasswordAsync(user.UserID, passwordHash, passwordSalt);
                if (!updateResult.IsSuccess)
                {
                    return StatusCode(500, updateResult);
                }

                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Password has been reset successfully.",
                    Code = "200"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred while resetting the password.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}
