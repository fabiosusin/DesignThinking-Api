using DTO.Hub.Order.Database;
using DTO.Hub.Order.Input;
using System;

namespace DTO.Integration.Asaas.Payments.Input
{
    public class AsaasCreateChargeInput
    {

        public AsaasCreateChargeInput(HubOrder order, string customerId)
        {
            if (order == null)
                return;

            Customer = customerId;
            DueDate = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
            Description = "Pedido " + order.Code;
            ExternalReference = order.Code.ToString();
            PostalService = false;
        }

        public string Customer { get; set; }
        public string BillingType { get; set; }
        public string DueDate { get; set; }
        public string Description { get; set; }
        public string ExternalReference { get; set; }
        public bool PostalService { get; set; }
        public decimal Value { get; set; }
        public AsaasDiscountInput Discount { get; set; }
        public AsaasCreditCard CreditCard { get; set; }
        public AsaasCreditCardHolderInfo CreditCardHolderInfo { get; set; }
    }

    public class AsaasDiscountInput : AsaasBaseValue
    {
        public AsaasDiscountInput(decimal value, int dueDataLimitDays) : base(value)
        {
            Value = value;
            DueDateLimitDays = dueDataLimitDays;
        }

        public int DueDateLimitDays { get; set; }
    }

    public class AsaasBaseValue
    {
        public AsaasBaseValue(decimal value) => Value = value;
        public decimal Value { get; set; }
    }

    public class AsaasCreditCard
    {
        public AsaasCreditCard() { }
        public AsaasCreditCard(HubCardData cardData)
        {
            if (cardData == null)
                return;

            HolderName = cardData.HolderName;
            Number = cardData.Number;
            ExpiryMonth = cardData.ExpiryMonth;
            ExpiryYear = cardData.ExpiryYear;
            Ccv = cardData.Ccv;
        }

        public string HolderName { get; set; }
        public string Number { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string Ccv { get; set; }
    }

    public class AsaasCreditCardHolderInfo
    {
        public AsaasCreditCardHolderInfo() { }
        public AsaasCreditCardHolderInfo(HubCardHolderInfo holderInfo)
        {
            if (holderInfo == null)
                return;

            Name = holderInfo.Name;
            CpfCnpj = holderInfo.CpfCnpj;
            MobilePhone = holderInfo.MobilePhone;
            Email = holderInfo.Email;
            AddressNumber = holderInfo.AddressNumber;
            PostalCode = holderInfo.PostalCode;
        }

        public string Name { get; set; }
        public string CpfCnpj { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string AddressNumber { get; set; }
        public string PostalCode { get; set; }
    }

}
