namespace DTO.External.Asaas.Input
{
    public class AsaasUpdateInput
    {
        public string Event { get; set; }
        public Payment Payment { get; set; }
    }
    public class Payment
    {
        public string Id { get; set; }
        public string PaymentLink { get; set; }
        public string BillingType { get; set; }
        public string Status { get; set; }
        public string InvoiceUrl { get; set; }
        public string BankSlipUrl { get; set; }
        public string TransactionReceiptUrl { get; set; }
    }
}
