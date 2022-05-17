using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Useful.Service;

namespace Business.API.General.Files.Word
{
    public class WordWrapper
    {

        public static string GeneratePdf(string file)
        {
            var libreOfficePath = EnvironmentService.LibreOfficePath;

            var procStartInfo = new ProcessStartInfo(libreOfficePath,
            string.Format($"--convert-to pdf {file} --outdir {EnvironmentService.DocumentBasePath}"))
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Environment.CurrentDirectory
            };

            var process = new Process { StartInfo = procStartInfo, };
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
                return file;

            File.Delete(file);
            
            var split = file.Split('.');
            return string.Join(".", split.Take(split.Length - 1)) + ".pdf";
        }
    }
}
