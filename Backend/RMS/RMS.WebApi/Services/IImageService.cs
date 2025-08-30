namespace RMS.WebApi.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile, string folderName);
        void DeleteImage(string imageUrl);
    }
}