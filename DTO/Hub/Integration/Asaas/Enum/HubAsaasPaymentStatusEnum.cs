namespace DTO.Hub.Integration.Asaas.Enum
{
    public enum HubAsaasPaymentStatusEnum
    {
        Pending,
        Confirmed,
        Received,
        ReceivedInCash,
        Overdue,
        RefundRequested,
        Refunded,
        ChargebackRequested,
        ChargebackDispute,
        AwaitingChargebackReversal,
        DunningRequested,
        DunningReceived,
        AwaitingRiskAnalysis
    }
}
