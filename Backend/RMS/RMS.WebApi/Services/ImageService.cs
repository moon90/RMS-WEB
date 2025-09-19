using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RMS.WebApi.Configurations;

namespace RMS.WebApi.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageSettings _imageSettings;

        public ImageService(IOptions<ImageSettings> imageSettings)
        {
            _imageSettings = imageSettings.Value;
        }

        public async Task<byte[]> GetImageBytesAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            // Validate file type
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!_imageSettings.AllowedTypes.Contains(fileExtension))
            {
                throw new ArgumentException($"Invalid file type. Allowed types are: {string.Join(", ", _imageSettings.AllowedTypes)}");
            }

            // Validate file size
            if (imageFile.Length > _imageSettings.MaxFileSizeMB * 1024 * 1024)
            {
                throw new ArgumentException($"File size exceeds the maximum limit of {_imageSettings.MaxFileSizeMB} MB.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
