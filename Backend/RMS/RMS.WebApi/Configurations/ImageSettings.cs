namespace RMS.WebApi.Configurations
{
    public class ImageSettings
    {
        public string[] AllowedTypes { get; set; }
        public int MaxFileSizeMB { get; set; }

        public int ProductImageWidth { get; set; }
        public int ProductImageHeight { get; set; }
        public int ThumbnailImageWidth { get; set; }
        public int ThumbnailImageHeight { get; set; }
        public int ProfilePictureWidth { get; set; }
        public int ProfilePictureHeight { get; set; }
    }
}