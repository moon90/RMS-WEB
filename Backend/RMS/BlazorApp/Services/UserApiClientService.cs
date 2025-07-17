using System.Net.Http.Json;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Domain.Dtos;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;
using RMS.Domain.Models.BaseModels;

namespace BlazorApp.Services;

using System.Net.Http.Json;
using System.Text.Json;

public class UserApiClientService
{
    private readonly HttpClient _httpClient;

    public UserApiClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("RMSApi");
    }

    public async Task<ResponseDto<PagedResult<UserDto>>> GetAllUsersAsync(int page = 1, int size = 10, string? searchQuery = null, string? sortColumn = null, string? sortDirection = null)
    {
        var url = $"api/users?pageNumber={page}&pageSize={size}";
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            url += $"&searchQuery={Uri.EscapeDataString(searchQuery)}";
        }
        if (!string.IsNullOrWhiteSpace(sortColumn))
        {
            url += $"&sortColumn={Uri.EscapeDataString(sortColumn)}";
        }
        if (!string.IsNullOrWhiteSpace(sortDirection))
        {
            url += $"&sortDirection={Uri.EscapeDataString(sortDirection)}";
        }

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ResponseDto<PagedResult<UserDto>>>();
            return error ?? new ResponseDto<PagedResult<UserDto>>
            {
                IsSuccess = false,
                Message = "Failed to retrieve users",
                Code = response.StatusCode.ToString()
            };
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<PagedResult<UserDto>>>();
        return result!;
    }

    public async Task<ResponseDto<UserDto>> GetUserByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/users/{id}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ResponseDto<UserDto>>();
            return error ?? new ResponseDto<UserDto>
            {
                IsSuccess = false,
                Message = $"Failed to retrieve user with ID {id}.",
                Code = response.StatusCode.ToString()
            };
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<UserDto>>();
        return result!;
    }

    public async Task<ResponseDto<UserDto>> GetUserByUsernameAsync(string username)
    {
        var response = await _httpClient.GetAsync($"api/users/username/{Uri.EscapeDataString(username)}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ResponseDto<UserDto>>();
            return error ?? new ResponseDto<UserDto>
            {
                IsSuccess = false,
                Message = $"Failed to retrieve user '{username}'.",
                Code = response.StatusCode.ToString()
            };
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<UserDto>>();
        return result!;
    }

    public async Task<ResponseDto<UserDto>> CreateUserAsync(UserCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users", dto);

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<UserDto>>();

        if (result is null)
        {
            return new ResponseDto<UserDto>
            {
                IsSuccess = false,
                Message = "Failed to deserialize user creation response.",
                Code = response.StatusCode.ToString()
            };
        }

        return result;
    }

    public async Task<ResponseDto<UserDto>> UpdateUserAsync(int id, UserUpdateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", dto);

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<UserDto>>();

        if (result is null)
        {
            return new ResponseDto<UserDto>
            {
                IsSuccess = false,
                Message = "Failed to deserialize user update response.",
                Code = response.StatusCode.ToString()
            };
        }

        return result;
    }

    public async Task<ResponseDto<string>> DeleteUserAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/users/{id}");

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<string>>();

        if (result is null)
        {
            return new ResponseDto<string>
            {
                IsSuccess = false,
                Message = "Failed to deserialize delete response.",
                Code = response.StatusCode.ToString()
            };
        }

        return result;
    }

    public async Task<ResponseDto<string>> AssignRoleToUserAsync(int userId, int roleId)
    {
        var response = await _httpClient.PostAsync($"api/users/{userId}/roles/{roleId}", null);

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<string>>();

        if (result is null)
        {
            return new ResponseDto<string>
            {
                IsSuccess = false,
                Message = "Failed to assign role.",
                Code = response.StatusCode.ToString()
            };
        }

        return result;
    }

    public async Task<ResponseDto<string>> UnassignRoleFromUserAsync(int userId, int roleId)
    {
        var response = await _httpClient.DeleteAsync($"api/users/{userId}/roles/{roleId}");

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<string>>();

        if (result is null)
        {
            return new ResponseDto<string>
            {
                IsSuccess = false,
                Message = "Failed to unassign role.",
                Code = response.StatusCode.ToString()
            };
        }

        return result;
    }

    public async Task<ResponseDto<string>> AssignRolesToUserAsync(int userId, List<int> roleIds)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/users/{userId}/assign-roles", roleIds);

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<string>>();

        if (result is null)
        {
            return new ResponseDto<string>
            {
                IsSuccess = false,
                Message = "Failed to assign roles.",
                Code = response.StatusCode.ToString()
            };
        }

        return result;
    }

    public async Task<ResponseDto<string>> UnassignRolesFromUserAsync(int userId, List<int> roleIds)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/users/{userId}/unassign-roles", roleIds);

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<string>>();

        if (result is null)
        {
            return new ResponseDto<string>
            {
                IsSuccess = false,
                Message = "Failed to unassign roles.",
                Code = response.StatusCode.ToString()
            };
        }

        return result;
    }

    public async Task<ResponseDto<string>> UploadProfilePictureAsync(int userId, Stream fileStream, string fileName, string contentType = "image/jpeg")
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

        content.Add(fileContent, "file", fileName);

        var response = await _httpClient.PostAsync($"api/users/{userId}/upload-profile-picture", content);

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<string>>();

        if (result is null)
        {
            return new ResponseDto<string>
            {
                IsSuccess = false,
                Message = "Failed to upload profile picture.",
                Code = response.StatusCode.ToString()
            };
        }

        return result;
    }

}
