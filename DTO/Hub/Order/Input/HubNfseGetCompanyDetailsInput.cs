using DTO.General.Base.Api.Output;
using DTO.Hub.Company.Database;
using System.Security.Cryptography.X509Certificates;

namespace DTO.Hub.Order.Input
{

    public class HubNfseGetCompanyDetailsInput : BaseApiOutput
    {
        public HubNfseGetCompanyDetailsInput(string message) : base(message) { }
        public HubNfseGetCompanyDetailsInput(HubCompany company, X509Certificate2 cert) : base(true)
        {
            Company = company;
            Certified = cert;
        }

        public HubCompany Company { get; set; }
        public X509Certificate2 Certified { get; set; }
    }
}
