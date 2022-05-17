using DTO.General.Image.Input;
using DTO.General.Image.PropertyAttribute;
using DTO.General.MimeType.Output;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Useful.Service;

namespace Utils.Extensions.Files.Images
{
    public class ImageFactory
    {
        private const string PicturesPath = "/Images";
        private const string PicturesPathPng = "/Png";
        private const string PicturesPathJpeg = "/Jpeg";

        public static ImageFormat SaveListResolutions(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return null;

            try
            {
                var jpegArray = new ImageResolutions().GetType().GetProperties()
                                    .Where(x => Attribute.IsDefined(x, typeof(ImageResolutionAttribute)));

                var pngArray = new ImageResolutions().GetType().GetProperties()
                                    .Where(x => Attribute.IsDefined(x, typeof(ImageResolutionAttribute)));

                var resultImageList = new List<ResultImage>();
                foreach (var item in jpegArray)
                    resultImageList.Add(GetImageConverted(base64, item.GetCustomAttribute<ImageResolutionAttribute>().Size, GetImagesEnum.Png | GetImagesEnum.Jpeg));

                if (resultImageList == null)
                    return null;

                var instanceJpeg = Activator.CreateInstance(typeof(ImageResolutions));
                var instancePng = Activator.CreateInstance(typeof(ImageResolutions));
                foreach (var item in jpegArray)
                    item.SetValue(instanceJpeg, resultImageList.Find(x => x.Size == item.GetCustomAttribute<ImageResolutionAttribute>().Size).ImageJpeg);

                foreach (var item in pngArray)
                    item.SetValue(instancePng, resultImageList.Find(x => x.Size == item.GetCustomAttribute<ImageResolutionAttribute>().Size).ImagePng);

                return new ImageFormat
                {
                    Jpeg = (ImageResolutions)instanceJpeg,
                    Png = (ImageResolutions)instancePng
                };
            }
            catch
            {
                return null;
            }
        }

        public static ResultImage SaveImageFromBase64(string base64, int size, GetImagesEnum type) => GetImageConverted(base64, size, type);

        private static ResultImage GetImageConverted(string base64, int size, GetImagesEnum type)
        {
            var imageByteArray = ConvertBase64ToByteArray(base64);
            if (imageByteArray == null)
                return null;

            var image = GetImageConverted(imageByteArray, size);
            var result = new ResultImage
            {
                ImageJpeg = type.HasFlag(GetImagesEnum.Jpeg) ? SaveImageOnServer(new ImageInput { ImageId = image?.Id, Image = image?.ImageJpeg }) : null,
                ImagePng = type.HasFlag(GetImagesEnum.Png) ? SaveImageOnServer(new ImageInput { ImageId = image?.Id, Image = image?.ImagePng }) : null,
                Size = size
            };

            return result;
        }

        private static ResultResizeImage GetImageConverted(byte[] imageContent, int resolution)
        {
            if (!HigherResolutionThanImage(imageContent, resolution))
                return null;

            return new ResultResizeImage
            {
                Id = Guid.NewGuid().ToString(),
                ImageJpeg = ImageTreatment.ResizeImage(imageContent, resolution, resolution, ImageType.Jpeg),
                ImagePng = ImageTreatment.ResizeImage(imageContent, resolution, resolution, ImageType.Png)
            };
        }

        private static bool HigherResolutionThanImage(byte[] imageData, int resolution)
        {
            var img = ImageTreatment.ConvertByteArrayToImage(imageData, ImageType.Jpeg);
            return img?.Height >= resolution || img?.Width >= resolution;
        }

        private static byte[] ConvertBase64ToByteArray(string base64) => string.IsNullOrEmpty(base64) ? null : Convert.FromBase64String(new Regex("data:image\\/.*;base64,").Replace(base64, ""));

        private static string SaveImageOnServer(ImageInput input)
        {
            if (string.IsNullOrEmpty(input?.ImageId) || input.Image == null)
                return null;

            var path = EnvironmentService.BaseFilesPath + PicturesPath;

            var folder = input.Type == FileType.Jpeg ? PicturesPathJpeg : PicturesPathPng;
            var extension = input.Type == FileType.Jpeg ? ".jpeg" : ".png";

            var samePath = folder + "/" + input.ImageId + extension;
            var result = path + samePath;

            var imagePath = path + samePath;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!Directory.Exists(path + folder))
                Directory.CreateDirectory(path + folder);

            try
            {
                var img = System.Drawing.Image.FromStream(new MemoryStream(input.Image));
                img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch
            {
                return null;
            }

            return $"{EnvironmentService.BaseURLFilesController}/Image/get-image?filePath={result}&contentType={(input.Type == FileType.Jpeg ? MimeTypeOutput.Jpeg : MimeTypeOutput.Png)}";
        }

        private class ImageInput
        {
            public byte[] Image { get; set; }
            public string ImageId { get; set; }
            public FileType Type { get; set; }
        }


        private class ResultResizeImage
        {
            public byte[] ImagePng { get; set; }
            public byte[] ImageJpeg { get; set; }
            public string Id { get; set; }
        }

        public class ResultImage
        {
            public string ImagePng { get; set; }
            public string ImageJpeg { get; set; }
            public int Size { get; set; }
        }

        [Flags]
        public enum GetImagesEnum
        {
            Unknown = 0,
            Jpeg = 1 << 0,
            Png = 1 << 1,
        }
    }
}
