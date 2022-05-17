using System;

namespace DTO.Integration.Asaas.Payments.Output
{
    public class AsaasCreateChargeOutput
    {
        public AsaasCreateChargeOutput() { }
        public AsaasCreateChargeOutput(string billingType) => BillingType = billingType;
        public string Id { get; set; }
        public string DateCreated { get; set; }
        public string Customer { get; set; }
        public string PaymentLink { get; set; }
        public string Description { get; set; }
        public string BillingType { get; set; }
        public string Status { get; set; }
        public string DueDate { get; set; }
        public string OriginalDueDate { get; set; }
        public string PaymentDate { get; set; }
        public string ClientPaymentDate { get; set; }
        public string InvoiceUrl { get; set; }
        public string InvoiceNumber { get; set; }
        public string ExternalReference { get; set; }
        public string CreditDate { get; set; }
        public string EstimatedCreditDate { get; set; }
        public string TransactionReceiptUrl { get; set; }
        public string NossoNumero { get; set; }
        public string BankSlipUrl { get; set; }
        public decimal Value { get; set; }
        public decimal NetValue { get; set; }
        public decimal? OriginalValue { get; set; }
        public decimal? InterestValue { get; set; }
        public bool PostalService { get; set; }
        public bool Deleted { get; set; }
        public bool Anticipated { get; set; }
        public AsaasCreditCardOutput CreditCard { get; set; }
        public AsaasDiscountOutput Discount { get; set; }
        public AsaasBasePayment Fine { get; set; }
        public AsaasBasePayment Interest { get; set; }
    }

    public class AsaasCreditCardOutput
    {
        public string CreditCardNumber { get; set; }
        public string CreditCardBrand { get; set; }
        public string CreditCardToken { get; set; }
    }

    public class AsaasDiscountOutput : AsaasBasePayment
    {
        public object LimitDate { get; set; }
        public int DueDateLimitDays { get; set; }
    }

    public class AsaasBasePayment
    {
        public double Value { get; set; }
        public string Type { get; set; }
    }
}
