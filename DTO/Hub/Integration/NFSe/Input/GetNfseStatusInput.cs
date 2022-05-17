using System.Security.Cryptography.X509Certificates;

namespace DTO.Hub.Integration.NFSe.Input
{
    public class GetNfseStatusInput : BaseNfseInput
    {
        public GetNfseStatusInput(string cnpj, string lot, X509Certificate2 certified): base(certified)
        {
            Cnpj = cnpj;
            Lot = lot;
        }

         public string Cnpj { get; set; }
        public string Lot { get; set; }
    }
}
