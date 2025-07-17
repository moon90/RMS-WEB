using System.Net.Http;
using System.Net.Http.Json;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Domain.Dtos;
using RMS.Domain.DTOs.RoleDTOs;
using RMS.Domain.DTOs.RoleDTOs.OutputDTOs;
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
        //var result = await response.Content.ReadFromJsonAsync<ResponseDto<PagedResult<UserDto>>>();
        var result = await response.Content.ReadFromJsonAsync<ResponseDto<PagedResult<RoleDto>>>();
        return result ?? new ResponseDto<PagedResult<RoleDto>>
        {
            IsSuccess = false,
            Message = "Failed to deserialize response."
        };
    }
}