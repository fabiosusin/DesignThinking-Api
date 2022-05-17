using System.Security.Cryptography.X509Certificates;

namespace DTO.Hub.Integration.NFSe.Input
{
    public class GetNfseDetailsInput : BaseNfseInput
    {
        public GetNfseDetailsInput(string cnpj, string key, X509Certificate2 cert) : base(cert)
        {
            Cnpj = cnpj;
            AccessKey = key;
        }

        public string Cnpj { get; set; }
        public string AccessKey { get; set; }
    }
}
