using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RMS.Application.Services.Processing
{
    public interface IImageProcessingService
    {
        Task<byte[]> ProcessImage(IFormFile imageFile, int width, int height);
    }
}
