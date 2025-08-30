using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq; // Added for LINQ
using System.Threading.Tasks;
using Microsoft.Extensions.Options; // Added
using RMS.WebApi.Configurations; // Added

namespace RMS.WebApi.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ImageSettings _imageSettings; // Added

        public ImageService(IWebHostEnvironment webHostEnvironment, IOptions<ImageSettings> imageSettings) // Modified
        {
            _webHostEnvironment = webHostEnvironment;
            _imageSettings = imageSettings.Value; // Added
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string folderName)
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

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/{folderName}/{uniqueFileName}"; // Return relative URL
        }

        public void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            var fileName = Path.GetFileName(imageUrl);
            var folderName = Path.GetDirectoryName(imageUrl).TrimStart('/');
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
