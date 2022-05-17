using DTO.Hub.Customer.Database;

namespace DTO.Integration.Asaas.Customer.Input
{
    public class AsaasCreateCustomerInput
    {
        public AsaasCreateCustomerInput(HubCustomer customer)
        {
            if (customer == null)
                return;

            Name = customer.Name;
            Email = customer.Email;
            CpfCnpj = customer.Document?.Data;
            Observations = customer.Notes;
            NotificationDisabled = true;

            if (customer.CellphoneData != null)
                MobilePhone = customer.CellphoneData.DDD + customer.CellphoneData.Number;

            if (customer.Address != null)
            {
                PostalCode = customer.Address.ZipCode;
                Address = customer.Address.Street;
                AddressNumber = customer.Address.Number;
                Complement = customer.Address.Complement;
                Province = customer.Address.Neighborhood;

            }
        }


        public string Name { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string CpfCnpj { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string AddressNumber { get; set; }
        public string Complement { get; set; }
        public string Province { get; set; }
        public string Observations { get; set; }
        public bool NotificationDisabled { get; set; }
    }
}
