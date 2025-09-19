using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RMS.WebApi.Services
{
    public interface IImageService
    {
        Task<byte[]> GetImageBytesAsync(IFormFile imageFile);
    }
}
