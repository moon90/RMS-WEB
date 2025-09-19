using Microsoft.IdentityModel.Tokens;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using RMS.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RMS.Application.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IUserService _userService;

        public TokenService(string secretKey, string issuer, string audience, IUserService userService)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
            _userService = userService;
        }

        public string GenerateToken(UserDto user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Add permissions as claims
                try
                {
                    var userPermissionsResult = _userService.GetRolePermissionsAsync(user.UserID).GetAwaiter().GetResult();
                    if (userPermissionsResult.IsSuccess && userPermissionsResult.Data != null)
                    {
                        foreach (var permission in userPermissionsResult.Data)
                        {
                            claims.Add(new Claim("Permission", permission));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving user permissions for token: {ex.Message}");
                    // Depending on desired behavior, you might re-throw or log and continue without permissions
                }

                var token = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(15), // Short-lived access token
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating token: {ex.Message}");
                throw; // Re-throw the exception as token generation is critical
            }
        }

        public string GenerateRefreshToken()
        {
            try
            {
                var randomNumber = new byte[32];
                using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating refresh token: {ex.Message}");
                throw; // Re-throw the exception as refresh token generation is critical
            }
        }

        public TokenValidationResultDto ValidateRefreshToken(string refreshToken, DateTime? expiryDate)
        {
            try
            {
                var user = _userService.GetUserByRefreshTokenAsync(refreshToken).Result;
                if (user == null)
                    return new TokenValidationResultDto
                    {
                        IsValid = false,
                        Message = "Token is null or empty"
                    };

                if (expiryDate == null)
                    return new TokenValidationResultDto
                    {
                        IsValid = false,
                        Message = "Token has no expiry date"
                    };

                if (expiryDate <= DateTime.UtcNow)
                    return new TokenValidationResultDto
                    {
                        IsValid = false,
                        IsExpired = true,
                        ExpiryDate = expiryDate,
                        Message = "Token has expired"
                    };

                return new TokenValidationResultDto
                {
                    IsValid = true,
                    IsExpired = false,
                    ExpiryDate = expiryDate,
                    Message = "Token is valid"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating refresh token: {ex.Message}");
                return new TokenValidationResultDto
                {
                    IsValid = false,
                    Message = "An error occurred during token validation.",
                    Details = ex.Message
                };
            }
        }


    }
}