using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;

namespace RMS.Application.Services.Processing
{
    public class ImageProcessingService : IImageProcessingService
    {
        public async Task<byte[]> ProcessImage(IFormFile imageFile, int width, int height)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                using (var image = await SixLabors.ImageSharp.Image.LoadAsync(memoryStream))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new SixLabors.ImageSharp.Size(width, height),
                        Mode = ResizeMode.Crop
                    }));

                    using (var outputStream = new MemoryStream())
                    {
                        await image.SaveAsPngAsync(outputStream);
                        return outputStream.ToArray();
                    }
                }
            }
        }
    }
}