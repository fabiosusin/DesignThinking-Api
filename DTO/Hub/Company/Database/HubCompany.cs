using DTO.General.Address.Input;
using DTO.General.Base.Database;
using DTO.Hub.Company.Enum;

namespace DTO.Hub.Company.Database
{
    public class HubCompany : BaseData
    {
        public HubCompany(string name, string depositId)
        {
            Name = name;
            DefaultDepositId = depositId;
            Default = true;
        }
        public bool Default { get; set; }
        public string Name { get; set; }
        public string DefaultDepositId { get; set; }
        public string Cnpj { get; set; }
        public string Im { get; set; }
        public string Site { get; set; }
        public string Email { get; set; }
        public HubCompanyTaxRegimeEnum TaxRegime { get; set; }
        public HubNfseCertified Certified { get; set; }
        public Address Address { get; set; }
    }

    public class HubNfseCertified
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
