using Microsoft.AspNetCore.Components;
using RMS.Domain.Dtos.UserDTOs.InputDTOs;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;
using RMS.Domain.Dtos;
using RMS.Domain.DTOs;
using System.Text.Json;
using Microsoft.JSInterop;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;

namespace BlazorApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigation;
        private readonly CustomAuthenticationStateProvider _authProvider;
        private readonly IJSRuntime _jsRuntime;

        public AuthService(IHttpClientFactory httpClientFactory, NavigationManager navigation, IJSRuntime jsRuntime, CustomAuthenticationStateProvider authProvider)
        {
            _httpClient = httpClientFactory.CreateClient("RMSApi");
            _jsRuntime = jsRuntime;
            _authProvider = authProvider;
            _navigation = navigation;
        }

        public async Task<ResponseDto<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);
            var result = await response.Content.ReadFromJsonAsync<ResponseDto<AuthResponseDto>>();

            if (result?.IsSuccess == true)
            {
                await _authProvider.MarkUserAsAuthenticated(result.Data.AccessToken);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", result.Data.RefreshToken);
                //var user = JsonSerializer.Deserialize<UserDto>(result.Data.User.ToString());

                string json = result.Data.User.ToString(); // your string
                var doc = JsonDocument.Parse(json);
                int userId = doc.RootElement.GetProperty("userID").GetInt32();

                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userId", userId);
            }

            return result!;
        }


        public async Task<ResponseDto<string>> LogoutAsync(int userId)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/logout", new { userId });
            await _authProvider.MarkUserAsLoggedOut();
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userId");
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
