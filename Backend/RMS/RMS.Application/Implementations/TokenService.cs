using Microsoft.IdentityModel.Tokens;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
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
        private readonly IUserRepository _userRepository;

        public TokenService(string secretKey, string issuer, string audience, IUserRepository userRepository)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
            _userRepository = userRepository;
        }

        public string GenerateToken(UserDto user)
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

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15), // Short-lived access token
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        //public TokenValidationResultDto ValidateRefreshToken(string refreshToken)
        //{
        //    // Example logic – replace with your actual token lookup
        //    var user = _userRepository.GetUserByRefreshTokenAsync(refreshToken).Result;
        //    if (user == null)
        //        return new TokenValidationResultDto { IsValid = false };

        //    var isExpired = user.RefreshTokenExpiry <= DateTime.UtcNow;

        //    return new TokenValidationResultDto
        //    {
        //        IsValid = !isExpired,
        //        IsExpired = isExpired,
        //        ExpiryDate = user.RefreshTokenExpiry
        //    };
        //}

        public TokenValidationResultDto ValidateRefreshToken(string refreshToken, DateTime? expiryDate)
        {
            var user = _userRepository.GetUserByRefreshTokenAsync(refreshToken).Result;
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


    }
}
