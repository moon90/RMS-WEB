using System.Net.Http;
using System.Net.Http.Json;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Domain.DTOs.RoleDTOs;
using RMS.Domain.DTOs.RoleDTOs.OutputDTOs;
using RMS.Domain.DTOs.RoleMenuDTOs.OutputDTOs; // Added
using RMS.Domain.Models.BaseModels;

namespace BlazorApp.Services;

public class RoleApiClientService
{
    private readonly HttpClient _httpClient;

    public RoleApiClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("RMSApi");
    }

    public async Task<ResponseDto<PagedResult<RoleDto>>> GetRolesAsync(int pageNumber = 1, int pageSize = 10)
    {
        var response = await _httpClient.GetAsync($"api/roles?pageNumber={pageNumber}&pageSize={pageSize}");

        if (!response.IsSuccessStatusCode)
        {
            return new ResponseDto<PagedResult<RoleDto>>
            {
                IsSuccess = false,
                Message = $"API call failed: {response.ReasonPhrase}"
            };
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<PagedResult<RoleDto>>>();
        if (result == null)
        {
            return new ResponseDto<PagedResult<RoleDto>>
            {
                IsSuccess = false,
                Message = "Failed to deserialize response or response was empty."
            };
        }

        return result;
    }

    public async Task<ResponseDto<IEnumerable<RMS.Domain.DTOs.RolePermissionDTOs.OutputDTOs.RolePermissionDto>>> GetRolePermissionsAsync(int roleId)
    {
        var response = await _httpClient.GetAsync($"api/roles/{roleId}/permissions");

        if (!response.IsSuccessStatusCode)
        {
            return new ResponseDto<IEnumerable<RMS.Domain.DTOs.RolePermissionDTOs.OutputDTOs.RolePermissionDto>>
            {
                IsSuccess = false,
                Message = $"API call failed: {response.ReasonPhrase}"
            };
        }
        var result = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<RMS.Domain.DTOs.RolePermissionDTOs.OutputDTOs.RolePermissionDto>>>();
        return result ?? new ResponseDto<IEnumerable<RMS.Domain.DTOs.RolePermissionDTOs.OutputDTOs.RolePermissionDto>>
        {
            IsSuccess = false,
            Message = "Failed to deserialize response."
        };
    }

    public async Task<ResponseDto<IEnumerable<RoleMenuDto>>> GetRoleMenusByRoleIdAsync(int roleId)
    {
        var response = await _httpClient.GetAsync($"api/roles/{roleId}/menus");

        if (!response.IsSuccessStatusCode)
        {
            return new ResponseDto<IEnumerable<RoleMenuDto>>
            {
                IsSuccess = false,
                Message = $"API call failed: {response.ReasonPhrase}"
            };
        }
        var result = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<RoleMenuDto>>>();
        return result ?? new ResponseDto<IEnumerable<RoleMenuDto>>
        {
            IsSuccess = false,
            Message = "Failed to deserialize response."
        };
    }
}
