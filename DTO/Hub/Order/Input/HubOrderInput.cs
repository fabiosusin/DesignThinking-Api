using DTO.Hub.Order.Database;
using DTO.Hub.Order.Enum;
using System.Collections.Generic;

namespace DTO.Hub.Order.Input
{
    public class HubOrderInput
    {
        public HubOrderInput() { }

        public string SellerId { get; set; }
        public string AccountPlanId { get; set; }
        public string DepositId { get; set; }
        public string CompanyId { get; set; }
        public HubCustomerOrderInput Customer { get; set; }
        public HubOrderAllyInput Ally { get; set; }
        public List<HubOrderInputPaymentData> Payments { get; set; }
    }

    public class HubCustomerOrderInput
    {
        public string Id { get; set; }
        public string DocumentBase64 { get; set; }
    }

    public class HubOrderInputPaymentData : HubOrderPaymentData
    {
        public HubOrderInputPaymentData() { }
        public HubOrderInputPaymentData(HubOrderPaymentFormEnum type, decimal value) : base(type, value) { }
        public HubOrderInputCardData CreditCard { get; set; }
    }

    public class HubOrderInputCardData
    {
        public HubCardHolderInfo HolderInfo { get; set; }
        public HubCardData CardData { get; set; }
    }

    public class HubCardHolderInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CpfCnpj { get; set; }
        public string PostalCode { get; set; }
        public string AddressNumber { get; set; }
        public string AddressComplement { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
    }

    public class HubCardData
    {
        public string HolderName { get; set; }
        public string Number { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string Ccv { get; set; }
    }

    public class HubOrderAllyInput
    {
        public HubOrderAllyInput(string id, string cnpj)
        {
            Id = id;
            Cnpj = cnpj;
        }
        public string Id { get; set; }
        public string Cnpj { get; set; }
    }
}
