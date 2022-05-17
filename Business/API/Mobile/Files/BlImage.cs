using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace Business.API.Mobile.Files
{
    public class BlImage
    {
        public static byte[] GetFile(string filePath) => File.ReadAllBytes(HttpUtility.UrlDecode(filePath));

        // TODO Implementar salvamento de imagens na AWS
        // Esse código é temporário até não salvarmos as imagens na AWS
        public static bool RemoveImage(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var pattern = @"filePath=(.*?)&";
            var filePath = Regex.Match(path, pattern).Value;
            filePath = Regex.Replace(filePath, @"filePath=", string.Empty);
            filePath = Regex.Replace(filePath, @"&", string.Empty);

            if (!File.Exists(Path.Combine(filePath)))
                return false;

            File.Delete(Path.Combine(filePath));
            return true;
        }
    }
}
