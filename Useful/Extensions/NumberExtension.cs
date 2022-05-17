using System;

namespace Useful.Extensions
{
    public static class NumberExtension
    {
        public static double ConvertSize(this double bytes, string type = null)
        {
            try
            {
                if (string.IsNullOrEmpty(type))
                    type = ConversionType.GB;

                const int CONVERSION_VALUE = 1024;
                //determine what conversion they want
                return type switch
                {
                    "BY" => bytes,//convert to bytes (default)
                    "KB" => bytes / CONVERSION_VALUE,//convert to kilobytes
                    "MB" => bytes / CalculateSquare(CONVERSION_VALUE),//convert to megabytes
                    "GB" => bytes / CalculateCube(CONVERSION_VALUE),//convert to gigabytes
                    _ => bytes,//default
                };
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Function to calculate the square of the provided number
        /// </summary>
        /// <param name="number">Int32 -> Number to be squared</param>
        /// <returns>Double -> THe provided number squared</returns>
        /// <remarks></remarks>
        private static double CalculateSquare(int number) => Math.Pow(number, 2);


        /// <summary>
        /// Function to calculate the cube of the provided number
        /// </summary>
        /// <param name="number">Int32 -> Number to be cubed</param>
        /// <returns>Double -> THe provided number cubed</returns>
        /// <remarks></remarks>
        private static double CalculateCube(int number) => Math.Pow(number, 3);

        public static class ConversionType
        {
            public const string By = "BY";
            public const string KB = "KB";
            public const string MB = "MB";
            public const string GB = "GB";
        }

        public static int RandomNumber(int lenght)
        {
            var min = "1";
            var max = "9";
            for (var i = 1; i < lenght; i++) { min += "0"; max += "9"; }

            return RandomNumber(int.Parse(min), int.Parse(max));
        }

        public static int RandomNumber(int min, int max) => new Random().Next(min, max);

        public static decimal SurfNumberConversion(this string strNumber)
        {
            var thousand = strNumber[..2];
            var cents = strNumber[2..];
            return decimal.Parse($"{thousand},{cents}");
        }

        public static int NumberToCents(this decimal number) => (int)(number * 100);

    }
}
