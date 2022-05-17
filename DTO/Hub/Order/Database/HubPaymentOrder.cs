using DTO.General.Base.Database;
using DTO.Hub.Integration.Asaas.Enum;
using DTO.Hub.Order.Enum;
using DTO.Hub.Order.Output;
using DTO.Integration.Asaas.Base.Enum;

namespace DTO.Hub.Order.Database
{
    public class HubPaymentOrder : BaseData
    {
        public HubPaymentOrder() { }
        public HubPaymentOrder(string orderId, decimal value, HubPaymentOrderType status, OrderAsaasData asaasData)
        {
            OrderId = orderId;
            Value = value;
            Status = status;
            AsaasData = asaasData;
        }

        public string OrderId { get; set; }
        public decimal Value { get; set; }
        public HubPaymentOrderType Status { get; set; }
        public OrderAsaasData AsaasData { get; set; }


        public static string GetPaymentString(HubOrderPaymentFormEnum type) => type switch
        {
            HubOrderPaymentFormEnum.BankSlip => HubOrderPaymentFormsOutput.BankSlip,
            HubOrderPaymentFormEnum.Pix => HubOrderPaymentFormsOutput.Pix,
            HubOrderPaymentFormEnum.CreditCard => HubOrderPaymentFormsOutput.CreditCard,
            _ => null
        };

        public static string GetAsaasPaymentString(HubOrderPaymentFormEnum type) => type switch
        {
            HubOrderPaymentFormEnum.BankSlip => AsaasPaymentTypeEnum.BankSlip,
            HubOrderPaymentFormEnum.Pix => AsaasPaymentTypeEnum.Pix,
            HubOrderPaymentFormEnum.CreditCard => AsaasPaymentTypeEnum.CreditCard,
            _ => null
        };

        public static string GetPaymentStringFromAsaas(string payment) => payment switch
        {
            AsaasPaymentTypeEnum.BankSlip => HubOrderPaymentFormsOutput.BankSlip,
            AsaasPaymentTypeEnum.Pix => HubOrderPaymentFormsOutput.Pix,
            AsaasPaymentTypeEnum.CreditCard => HubOrderPaymentFormsOutput.CreditCard,
            _ => null
        };

        public static HubPaymentOrderType GetStatusFromAsaasStatus(HubAsaasPaymentStatusEnum status) => status switch
        {
            HubAsaasPaymentStatusEnum.Confirmed => HubPaymentOrderType.Paid,
            HubAsaasPaymentStatusEnum.Received => HubPaymentOrderType.Paid,
            HubAsaasPaymentStatusEnum.Overdue => HubPaymentOrderType.Overdue,
            HubAsaasPaymentStatusEnum.Refunded => HubPaymentOrderType.Canceled,
            _ => HubPaymentOrderType.AwaitingPayment
        };

        public static HubPaymentOrderType GetStatusFromAsaasStatusStr(string status) => status switch
        {
            AsaasPaymentStatusEnum.AwaitingRiskAnalysis => HubPaymentOrderType.AwaitingPayment,
            AsaasPaymentStatusEnum.DunningRequested => HubPaymentOrderType.AwaitingPayment,
            AsaasPaymentStatusEnum.Pending => HubPaymentOrderType.AwaitingPayment,
            AsaasPaymentStatusEnum.DunningReceived => HubPaymentOrderType.Paid,
            AsaasPaymentStatusEnum.Confirmed => HubPaymentOrderType.Paid,
            AsaasPaymentStatusEnum.Received => HubPaymentOrderType.Paid,
            AsaasPaymentStatusEnum.ReceivedInCash => HubPaymentOrderType.Paid,
            AsaasPaymentStatusEnum.Overdue => HubPaymentOrderType.Overdue,
            _ => HubPaymentOrderType.Canceled
        };
    }

    public class OrderAsaasData
    {
        public OrderAsaasData() { }
        public OrderAsaasData(HubOrderPaymentFormEnum type) => PaymentType = type;
        public OrderAsaasData(HubOrderPaymentFormEnum type, string id, HubBankSlipOutput slipOutput)
        {
            PaymentType = type;
            AsaasId = id;

            if (slipOutput == null)
                return;

            InvoiceUrl = slipOutput.InvoiceUrl;
            BankUrlSlip = slipOutput.SlipUrl;
        }

        public OrderAsaasData(HubOrderPaymentFormEnum type, string id, HubPixOutput pixOutput)
        {
            PaymentType = type;
            AsaasId = id;

            if (pixOutput == null)
                return;

            InvoiceUrl = pixOutput.InvoiceUrl;
        }

        public OrderAsaasData(HubOrderPaymentFormEnum type, string id, HubCreditCardOutput creditCardOutput)
        {
            PaymentType = type;
            AsaasId = id;

            if (creditCardOutput == null)
                return;

            InvoiceUrl = creditCardOutput.InvoiceUrl;
            TransactionReceiptUrl = creditCardOutput.TransactionUrl;
        }

        public OrderAsaasData(HubOrderPaymentFormEnum type, string id, string invoiceUrl, string bankSlip, string transactionUrl)
        {
            PaymentType = type;
            AsaasId = id;
            BankUrlSlip = bankSlip;
            InvoiceUrl = invoiceUrl;
            TransactionReceiptUrl = TransactionReceiptUrl;
        }

        public string AsaasId { get; set; }
        public HubOrderPaymentFormEnum PaymentType { get; set; }
        public string InvoiceUrl { get; set; }
        public string BankUrlSlip { get; set; }
        public string TransactionReceiptUrl { get; set; }
    }
}
