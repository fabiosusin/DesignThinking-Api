using System.Security.Cryptography.X509Certificates;

namespace DTO.Hub.Integration.NFSe.Input
{
    public class BaseNfseInput
    {
        public BaseNfseInput(X509Certificate2 cert) => Certified = cert;
        public X509Certificate2 Certified { get; set; }
    }
}
