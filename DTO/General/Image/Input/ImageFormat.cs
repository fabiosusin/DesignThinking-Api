using DTO.General.Image.PropertyAttribute;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace DTO.General.Image.Input
{
    public class ImageFormat
    {
        public ImageResolutions Png { get; set; } = new ImageResolutions();
        public ImageResolutions Jpeg { get; set; } = new ImageResolutions();

        [BsonIgnore]
        public string ImageUrl { get; set; }

        public string GetImage(ListResolutionsSize size, FileType type) =>
            GetImage((int)size, type);

        public string GetImage(int size, FileType type)
        {
            var result = string.Empty;

            size = size < (int)ListResolutionsSize.Url32 ? (int)ListResolutionsSize.Url32 : size;
            switch (type)
            {
                case FileType.Jpeg:
                    result = ImageResult(Jpeg, size);
                    break;
                case FileType.Png:
                    result = ImageResult(Png, size);
                    break;
            }

            return result;
        }

        private static string ImageResult(ImageResolutions resolutions, int size)
        {
            if (resolutions == null)
                return string.Empty;

            var array = resolutions.GetType().GetProperties()
                .Where(x => Attribute.IsDefined(x, typeof(ImageResolutionAttribute)) && !string.IsNullOrEmpty(x?.GetValue(resolutions) as string))
                .Select(x => new { x?.GetCustomAttribute<ImageResolutionAttribute>()?.Size, ImageUrl = x?.GetValue(resolutions) as string });
            var nearest = array.OrderBy(x => Math.Abs((long)x?.Size - size)).FirstOrDefault();

            return nearest?.ImageUrl;
        }

    }

    public class ImageResolutions
    {
        [ImageResolution(ListResolutionsSize.Url32)]
        public string Url32 { get; set; }

        [ImageResolution(ListResolutionsSize.Url64)]
        public string Url64 { get; set; }

        [ImageResolution(ListResolutionsSize.Url128)]
        public string Url128 { get; set; }

        [ImageResolution(ListResolutionsSize.Url256)]
        public string Url256 { get; set; }

        [ImageResolution(ListResolutionsSize.Url512)]
        public string Url512 { get; set; }

        [ImageResolution(ListResolutionsSize.Url1024)]
        public string Url1024 { get; set; }

        [ImageResolution(ListResolutionsSize.Url2048)]
        public string Url2048 { get; set; }
    }

    public enum ListResolutionsSize
    {
        Url32 = 32 << 0,
        Url64 = 32 << 1,
        Url128 = 32 << 2,
        Url256 = 32 << 3,
        Url512 = 32 << 4,
        Url1024 = 32 << 5,
        Url2048 = 32 << 6
    }

    public enum NonStandardListResolutionsSize
    {
        Url185 = 185,
        Url800 = 800,
        Url1000 = 1000,
    }

    public enum FileType
    {
        Png = 0,
        Jpeg = 1
    }
}
