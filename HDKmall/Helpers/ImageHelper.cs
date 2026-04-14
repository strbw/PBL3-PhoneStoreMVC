namespace HDKmall.Helpers
{
    public static class ImageHelper
    {
        public static string GetImageUrl(string? imageUrl, string defaultImage = "/img/default.png")
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return defaultImage;
            }

            // Fix backslashes to forward slashes
            imageUrl = imageUrl.Replace("\\", "/");

            // Extract valid path if it's a bracketed string like [\"/uploads/image.png\"]
            if (imageUrl.StartsWith("[") && imageUrl.EndsWith("]"))
            {
                imageUrl = imageUrl.Trim('[', ']', '\"');
            }

            // Add leading slash if it's a relative path and doesn't start with http or /
            if (!imageUrl.StartsWith("http", System.StringComparison.OrdinalIgnoreCase) && !imageUrl.StartsWith("/"))
            {
                imageUrl = "/" + imageUrl;
            }

            return imageUrl;
        }
    }
}
