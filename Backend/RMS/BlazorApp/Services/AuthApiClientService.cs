using System.Net.Http.Json;
using RMS.Domain.Dtos;
using RMS.Domain.Dtos.UserDTOs.InputDTOs;
using System.Text.Json;
using RMS.Domain.DTOs;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;

namespace BlazorApp.Services
{
    public class AuthApiClientService
    {
        private readonly HttpClient _httpClient;

        public AuthApiClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("RMSApi");
        }

        public async Task<ResponseDto<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);
            return await ParseResponse<AuthResponseDto>(response);
        }

        public async Task<ResponseDto<string>> LogoutAsync(int userId)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/logout", new { userId });
            return await ParseResponse<string>(response);
        }

        public async Task<ResponseDto<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/refresh-token", dto);
            return await ParseResponse<AuthResponseDto>(response);
        }

        public async Task<ResponseDto<object>> RegisterAsync(UserCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);
            return await ParseResponse<object>(response);
        }

        public async Task<ResponseDto<object>> ForgotPasswordAsync(ForgetPasswordDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/forgot-password", dto);
            return await ParseResponse<object>(response);
        }

        public async Task<ResponseDto<object>> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", dto);
            return await ParseResponse<object>(response);
        }

        private async Task<ResponseDto<T>> ParseResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                var result = JsonSerializer.Deserialize<ResponseDto<T>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null)
                    throw new ApplicationException("Invalid response format.");

                return result;
            }
            catch
            {
                throw new ApplicationException("Failed to parse API response: " + content);
            }
        }
    }
}
