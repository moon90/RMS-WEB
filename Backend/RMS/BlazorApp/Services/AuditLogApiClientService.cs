using System.Net.Http.Json;
using RMS.Application.DTOs;
using RMS.Domain.Entities;

namespace BlazorApp.Services;

public class AuditLogApiClientService
{
    private readonly HttpClient _httpClient;

    public AuditLogApiClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("RMSApi");
    }

    public async Task<ResponseDto<IEnumerable<AuditLog>>> GetAllAuditLogsAsync()
    {
        var response = await _httpClient.GetAsync("api/auditlogs");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<AuditLog>>>();
            return error ?? new ResponseDto<IEnumerable<AuditLog>>
            {
                IsSuccess = false,
                Message = "Failed to retrieve audit logs",
                Code = response.StatusCode.ToString()
            };
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<AuditLog>>>();
        return result!;
    }
}
