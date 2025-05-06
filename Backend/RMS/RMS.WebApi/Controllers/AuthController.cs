using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Application.Helpers;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos.UserDTOs.InputDTOs;
using RMS.Domain.DTOs;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;

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
                var user = await _userService.AuthenticateAsync(loginDto.UserName, loginDto.Password);
                if (user == null)
                    return Unauthorized(new ErrorResponseDto
                    {
                        Message = "Invalid username or password.",
                        ErrorCode = "AUTH_INVALID_CREDENTIALS"
                    });

                // Check role permissions
                var rolePermissions = await _userService.GetRolePermissionsAsync(user.UserID);
                if (!rolePermissions.Any())
                    return Unauthorized(new ErrorResponseDto
                    {
                        Message = "User has no role permissions.",
                        ErrorCode = "AUTH_NO_ROLE_PERMISSIONS"
                    });

                // Check menu permissions
                var menuPermissions = await _userService.GetMenuPermissionsAsync(user.UserID);
                if (!menuPermissions.Any())
                    return Unauthorized(new ErrorResponseDto
                    {
                        Message = "User has no menu permissions.",
                        ErrorCode = "AUTH_NO_MENU_PERMISSIONS"
                    });

                // Generate tokens
                var userDto = _mapper.Map<UserDto>(user);
                var accessToken = _tokenService.GenerateToken(userDto);
                var refreshToken = _tokenService.GenerateRefreshToken();

                // Save the refresh token in the database
                await _userService.UpdateRefreshTokenAsync(user.UserID, refreshToken, DateTime.UtcNow.AddDays(7));

                // Return success response
                return Ok(new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    User = userDto,
                    RolePermissions = rolePermissions,
                    MenuPermissions = menuPermissions
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new ErrorResponseDto
                {
                    Message = "An unexpected error occurred.",
                    ErrorCode = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message // Include exception details for debugging (optional)
                });
            }
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDto userCreateDto)
        {
            try
            {
                // Create the user (password hashing and mapping are handled by UserService)
                var userId = await _userService.CreateUserAsync(userCreateDto);

                // Return a 201 Created response with the user's ID
                return CreatedAtAction(nameof(GetUserById), new { id = userId }, new { UserId = userId });
            }
            catch (ValidationException ex)
            {
                // Handle validation errors
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, "An error occurred while registering the user.");
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserDto>(user));
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var user = await _userService.GetUserByRefreshTokenAsync(refreshTokenDto.RefreshToken);
            if (user == null)
                return Unauthorized("Invalid refresh token.");

            var validationResult = _tokenService.ValidateRefreshToken(refreshTokenDto.RefreshToken, user.RefreshTokenExpiry);

            if (!validationResult.IsValid)
                return Unauthorized(validationResult.Message);

            var userDto = _mapper.Map<UserDto>(user);
            var newAccessToken = _tokenService.GenerateToken(userDto);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            await _userService.UpdateRefreshTokenAsync(user.UserID, newRefreshToken, DateTime.UtcNow.AddDays(7));

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(int userId)
        {
            // Invalidate the refresh token
            await _userService.UpdateRefreshTokenAsync(userId, null, null);
            return Ok("Logged out successfully.");
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordDto forgetPasswordDto)
        {
            var user = await _userService.GetUserByEmailAsync(forgetPasswordDto.Email);
            if (user == null)
                return NotFound(new { message = "Email not registered." });

            var resetToken = Guid.NewGuid().ToString();
            var expiryTime = DateTime.UtcNow.AddMinutes(15);

            await _userService.SetPasswordResetTokenAsync(user.UserID, resetToken, expiryTime);

            // Send email
            var resetLink = $"https://yourfrontendapp.com/reset-password?token={resetToken}&email={forgetPasswordDto.Email}";

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "PasswordResetTemplate.html");
            var body = await System.IO.File.ReadAllTextAsync(templatePath);
            body = body.Replace("{{ResetLink}}", resetLink);

            //await _emailSender.SendEmailAsync(user.Email, "Password Reset Request",
            //    $"<h3>Password Reset</h3><p>Click <a href='{resetLink}'>here</a> to reset your password.</p>");

            await _emailSender.SendEmailAsync(user.Email, "Password Reset Request", body);

            return Ok(new { message = "Password reset email sent." });
        }

        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userService.GetUserByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return NotFound(new { message = "Invalid email." });

            if (user.PasswordResetToken != resetPasswordDto.ResetToken || user.PasswordResetTokenExpiry < DateTime.UtcNow)
                return BadRequest(new { message = "Invalid or expired reset token." });

            // Hash the new password
            var (passwordHash, passwordSalt) = PasswordHelper.CreatePasswordHash(resetPasswordDto.NewPassword);
            await _userService.UpdateUserPasswordAsync(user.UserID, passwordHash, passwordSalt);

            return Ok(new { message = "Password has been reset successfully." });
        }


        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            return PasswordHelper.VerifyPassword(password, storedHash, storedSalt);
        }
    }
}
