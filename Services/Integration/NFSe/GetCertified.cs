using System.Security.Cryptography.X509Certificates;
using Useful.Service;

namespace Services.Integration.NFSe
{
    public class GetCertified
    {
        public static X509Certificate2 Certified(string name, string password)
        {
            try { return new X509Certificate2($"{EnvironmentService.BaseFilesPath}/Certified/{name}", password); } catch  { return null; }
        }
    }
}
