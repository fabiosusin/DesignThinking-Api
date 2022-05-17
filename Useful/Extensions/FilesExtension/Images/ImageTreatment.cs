using System;
using System.Drawing;
using System.IO;
using System.ComponentModel;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Utils.Extensions.Files.Images
{
    public class ImageTreatment
    {
        public static void KeepRatioResise(int originalWidth, int originalHeight, ref int targetWidth, ref int targetHeight)
        {
            if (originalWidth > originalHeight)
                targetHeight = (targetWidth * originalHeight) / originalWidth;
            else
                targetWidth = (targetHeight * originalWidth) / originalHeight;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format) => ImageCodecInfo.GetImageDecoders()?.FirstOrDefault(x => x.FormatID == format.Guid);

        public static Image ConvertByteArrayToImage(byte[] byteArray, ImageType type) => byteArray == null || byteArray.Length == 0 ? null : (Image.FromStream(new MemoryStream(byteArray)));

        public static byte[] ResizeImage(Image image, int width, int height, ImageType type, bool keepRatio = true)
        {
            using (image)
            {
                if (keepRatio)
                    KeepRatioResise(image.Width, image.Height, ref width, ref height);
                using (var newImage = new Bitmap(width, height))
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    if (type == ImageType.Jpeg)
                        graphics.Clear(Color.White);

                    graphics.DrawImage(image, new Rectangle(0, 0, width, height));

                    if (type == ImageType.Png)
                    {
                        using (var ms = new MemoryStream())
                        {
                            newImage.Save(ms, ImageFormat.Png);
                            return ms.ToArray();
                        }
                    }
                    else
                    {
                        using (var ms = new MemoryStream())
                        {
                            var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                            var myEncoder = Encoder.Quality;
                            var myEncoderParameters = new EncoderParameters(1);

                            var myEncoderParameter = new EncoderParameter(myEncoder, 75L); //70 qualidade para jpeg
                            myEncoderParameters.Param[0] = myEncoderParameter;

                            newImage.Save(ms, jpgEncoder, myEncoderParameters);
                            return ms.ToArray();
                        }
                    }
                }
            }
        }
        
        public static byte[] ResizeImage(Stream image, int width, int height, ImageType type, bool keepRatio = true) =>
            ResizeImage(Image.FromStream(image), width, height, type, keepRatio);

        public static byte[] ResizeImage(byte[] image, int width, int height, ImageType type, bool keepRatio = true) =>
            ResizeImage(new MemoryStream(image), width, height, type, keepRatio);

        public static byte[] ResizeImage(string image64, int width, int height, ImageType type, bool keepRatio = true) =>
            ResizeImage(Convert.FromBase64String(image64), width, height, type, keepRatio);
    }

    public enum ImageType
    {
        [Description("")]
        Unknown = 0,
        [Description("jpeg")]
        Jpeg = 1,
        [Description("png")]
        Png = 2
    }
}
