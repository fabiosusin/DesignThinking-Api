
namespace DTO.Integration.Asaas.Base.Enum
{
    public static class AsaasPaymentStatusEnum
    {
        public const string Pending = "PENDING";
        public const string Confirmed = "CONFIRMED";
        public const string Received = "RECEIVED";
        public const string ReceivedInCash = "RECEIVED_IN_CASH";
        public const string Overdue = "OVERDUE";
        public const string RefundRequested = "REFUND_REQUESTED";
        public const string Refunded = "REFUNDED";
        public const string ChargebackRequested = "CHARGEBACK_REQUESTED";
        public const string ChargebackDispute = "CHARGEBACK_DISPUTE";
        public const string AwaitingChargebackReversal = "AWAITING_CHARGEBACK_REVERSAL";
        public const string DunningRequested = "DUNNING_REQUESTED";
        public const string DunningReceived = "DUNNING_RECEIVED";
        public const string AwaitingRiskAnalysis = "AWAITING_RISK_ANALYSIS";
    }
}
