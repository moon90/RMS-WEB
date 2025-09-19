using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserDto user);
        string GenerateRefreshToken();
        TokenValidationResultDto ValidateRefreshToken(string refreshToken, DateTime? expiryDate);
    }
}
