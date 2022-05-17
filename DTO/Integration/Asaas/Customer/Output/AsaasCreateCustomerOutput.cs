﻿namespace DTO.Integration.Asaas.Customer.Output
{
    public class AsaasCreateCustomerOutput
    {
        public string Id { get; set; }
        public string DateCreated { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Address { get; set; }
        public string AddressNumber { get; set; }
        public string Complement { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string CpfCnpj { get; set; }
        public string PersonType { get; set; }
        public string AdditionalEmails { get; set; }
        public string ExternalReference { get; set; }
        public string Observations { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int City { get; set; }
        public bool Deleted { get; set; }
        public bool NotificationDisabled { get; set; }
        public bool ForeignCustomer { get; set; }
    }
}
