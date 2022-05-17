using DTO.General.Address.Input;
using DTO.Hub.Company.Database;
using DTO.Hub.Customer.Database;
using System.Security.Cryptography.X509Certificates;

namespace DTO.Hub.Integration.NFSe.Input
{
    public class SendNfseInput : BaseNfseInput
    {
        public SendNfseInput(long number, decimal price, NfseServiceProviderInput provider, NfseCustomerInput customer, X509Certificate2 cert) : base(cert)
        {
            Number = number;
            TotalPrice = price;
            Provider = provider;
            Customer = customer;
        }

        public long Number { get; set; }
        public decimal TotalPrice { get; set; }
        public NfseServiceProviderInput Provider { get; set; }
        public NfseCustomerInput Customer { get; set; }
    }

    public class NfseServiceProviderInput
    {
        public NfseServiceProviderInput(HubCompany company)
        {
            if (company == null)
                return;

            Address = company.Address;
            Name = company.Name;
            Cnpj = company.Cnpj;
            Im = company.Im;
            TaxRegime = ((int)company.TaxRegime).ToString();
            Site = company.Site;
            Email = company.Email;
        }

        public string Name { get; set; }
        public string Cnpj { get; set; }
        public string Im { get; set; }
        public string TaxRegime { get; set; }
        public string Site { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }

    public class NfseCustomerInput
    {
        public NfseCustomerInput(HubCustomer customer)
        {
            if (customer == null)
                return;

            Name = customer.Name;
            CpfCnpj = customer.Document.Data;
            Address = customer.Address;
        }

        public string Name { get; set; }
        public string CpfCnpj { get; set; }
        public Address Address { get; set; }
    }
}
