using DTO.General.Base.Api.Output;
using DTO.Hub.Cellphone.Database;
using DTO.Hub.Cellphone.Enum;
using DTO.Hub.Integration.Asaas.Enum;
using DTO.Hub.Order.Enum;
using DTO.Integration.Asaas.Base.Enum;
using DTO.Integration.Asaas.Base.Output;
using DTO.Integration.Asaas.Payments.Output;
using System.Collections.Generic;

namespace DTO.Hub.Order.Output
{
    public class HubCreateOrderOutput : BaseApiOutput
    {
        public HubCreateOrderOutput(string msg) : base(msg) { }
        public HubCreateOrderOutput(string msg, string orderId) : base(msg) => Order = new(orderId);
        public HubCreateOrderOutput(bool success, string orderId) : base(success)
        {
            Order = new(orderId);
            Products = new List<HubProductOrderDetailsOutput>();
        }

        public HubOrderCreationResult Order { get; set; }
        public List<HubProductOrderDetailsOutput> Products { get; set; }
    }

    public class HubOrderCreationResult
    {
        public HubOrderCreationResult(string id)
        {
            Id = id;
            Status = HubOrderStatusEnum.Created;
            Payments = new List<HubOrderCreationChargeOutput>();
        }

        public string Id { get; set; }
        public List<HubOrderCreationChargeOutput> Payments { get; set; }
        public HubOrderStatusEnum Status { get; set; }
    }

    public class HubOrderCreationChargeOutput
    {
        public HubOrderCreationChargeOutput(AsaasDefaultErrorResult error) => Error = error;
        public HubOrderCreationChargeOutput(AsaasDefaultErrorResult error, string paymentForm)
        {
            PaymentForm = paymentForm;
            Error = error;
        }
        public HubOrderCreationChargeOutput(AsaasDefaultErrorResult error, string paymentForm, decimal value)
        {
            PaymentForm = paymentForm;
            Error = error;
            Value = value;
        }
        public HubOrderCreationChargeOutput(string paymentForm, HubAsaasPaymentStatusEnum status)
        {
            PaymentForm = paymentForm;
            Status = status;
        }
        public HubOrderCreationChargeOutput(string paymentForm, HubAsaasPaymentStatusEnum status, decimal value)
        {
            PaymentForm = paymentForm;
            Status = status;
            Value = value;
        }
        public HubOrderCreationChargeOutput(HubBankSlipOutput ticket, string payment, AsaasCreateChargeOutput asaas)
        {
            BankSlip = ticket;
            PaymentForm = payment;

            if (asaas == null)
                return;

            AsaasId = asaas.Id;
            Status = GetStatusFromAsaas(asaas.Status);
        }

        public HubOrderCreationChargeOutput(HubPixOutput pix, string payment, AsaasCreateChargeOutput asaas)
        {
            Pix = pix;
            PaymentForm = payment;

            if (asaas == null)
                return;

            AsaasId = asaas.Id;
            Status = GetStatusFromAsaas(asaas.Status);
        }

        public HubOrderCreationChargeOutput(HubCreditCardOutput creditCard, string payment, AsaasCreateChargeOutput asaas)
        {
            CreditCard = creditCard;
            PaymentForm = payment;

            if (asaas == null)
                return;

            AsaasId = asaas.Id;
            Status = GetStatusFromAsaas(asaas.Status);
        }

        public string PaymentOrderId { get; set; }
        public string AsaasId { get; set; }
        public string PaymentForm { get; set; }
        public HubAsaasPaymentStatusEnum Status { get; set; }
        public decimal Value { get; set; }
        public AsaasDefaultErrorResult Error { get; set; }
        public HubBankSlipOutput BankSlip { get; set; }
        public HubPixOutput Pix { get; set; }
        public HubCreditCardOutput CreditCard { get; set; }

        public static HubAsaasPaymentStatusEnum GetStatusFromAsaas(string payment) => payment switch
        {
            AsaasPaymentStatusEnum.Pending => HubAsaasPaymentStatusEnum.Pending,
            AsaasPaymentStatusEnum.Confirmed => HubAsaasPaymentStatusEnum.Confirmed,
            AsaasPaymentStatusEnum.Received => HubAsaasPaymentStatusEnum.Received,
            AsaasPaymentStatusEnum.ReceivedInCash => HubAsaasPaymentStatusEnum.ReceivedInCash,
            AsaasPaymentStatusEnum.Overdue => HubAsaasPaymentStatusEnum.Overdue,
            AsaasPaymentStatusEnum.RefundRequested => HubAsaasPaymentStatusEnum.RefundRequested,
            AsaasPaymentStatusEnum.Refunded => HubAsaasPaymentStatusEnum.Refunded,
            AsaasPaymentStatusEnum.ChargebackRequested => HubAsaasPaymentStatusEnum.ChargebackRequested,
            AsaasPaymentStatusEnum.ChargebackDispute => HubAsaasPaymentStatusEnum.ChargebackDispute,
            AsaasPaymentStatusEnum.AwaitingChargebackReversal => HubAsaasPaymentStatusEnum.AwaitingChargebackReversal,
            AsaasPaymentStatusEnum.DunningRequested => HubAsaasPaymentStatusEnum.DunningRequested,
            AsaasPaymentStatusEnum.DunningReceived => HubAsaasPaymentStatusEnum.DunningReceived,
            AsaasPaymentStatusEnum.AwaitingRiskAnalysis => HubAsaasPaymentStatusEnum.AwaitingRiskAnalysis,
            _ => HubAsaasPaymentStatusEnum.Pending
        };
    }

    public class HubPixOutput
    {
        public HubPixOutput(AsaasGetQrCodeResultOutput output, string invoiceUrl)
        {
            InvoiceUrl = invoiceUrl;
            if (output?.QrCode == null)
                return;

            QrCode = !string.IsNullOrEmpty(output.QrCode.EncodedImage) ? "data:image/jpeg;base64," + output.QrCode.EncodedImage : null;
            Code = output.QrCode.Payload;
        }

        public string InvoiceUrl { get; set; }
        public string QrCode { get; set; }
        public string Code { get; set; }
    }

    public class HubCreditCardOutput
    {
        public HubCreditCardOutput(string invoiceUrl, string transactionUrl)
        {
            InvoiceUrl = invoiceUrl;
            TransactionUrl = transactionUrl;
        }
        public string InvoiceUrl { get; set; }
        public string TransactionUrl { get; set; }
    }

    public class HubBankSlipOutput
    {
        public HubBankSlipOutput(string slipUrl, string invoiceUrl)
        {
            InvoiceUrl = invoiceUrl;
            SlipUrl = slipUrl;
        }

        public string InvoiceUrl { get; set; }
        public string SlipUrl { get; set; }
    }

    public class HubProductOrderResultCellphoneData
    {
        public HubProductOrderResultCellphoneData(string errorMessage) => ErrorMessage = errorMessage;
        public HubProductOrderResultCellphoneData(string errorMessage, string productOrderId, string mobilePlanId)
        {
            MobilePlanId = mobilePlanId;
            ErrorMessage = errorMessage;
            ProductOrderId = productOrderId;
        }

        public HubProductOrderResultCellphoneData(HubCellphoneManagement input, string productOrderId)
        {
            if (input == null)
                return;

            Success = input.Status == HubCellphoneManagementStatusEnum.Completed || input.Status == HubCellphoneManagementStatusEnum.AwaitingChargePayment;
            ManagementId = input.Id;
            ICCID = input.ChipSerial;
            ProductOrderId = productOrderId;
            ErrorMessage = GetErrorMessage(input.Status);
            Status = input.Status;
            if (input.CellphoneData != null)
            {
                DDD = input.CellphoneData.DDD;
                Number = input.CellphoneData.DDD + input.CellphoneData.Number;
            }
        }

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string ManagementId { get; set; }
        public string ICCID { get; set; }
        public string DDD { get; set; }
        public string Number { get; set; }
        public string ProductOrderId { get; set; }
        public string MobilePlanId { get; set; }
        public HubCellphoneManagementStatusEnum Status { get; set; }

        private static string GetErrorMessage(HubCellphoneManagementStatusEnum mode) => mode switch
        {
            HubCellphoneManagementStatusEnum.Create => "Erro ao adicionar Cliente!",
            HubCellphoneManagementStatusEnum.AddSurfCustomer => "Erro ao ativar chip!",
            HubCellphoneManagementStatusEnum.ChipActive => "Erro ao erro ao criar a portabilidade!",
            HubCellphoneManagementStatusEnum.PortabilityActive => "Erro ao finalizar!",
            HubCellphoneManagementStatusEnum.Canceled => "Cancelado!",
            HubCellphoneManagementStatusEnum.Defaulter => "Cliente Inadimplente!",
            HubCellphoneManagementStatusEnum.AwaitingPayment => "Chip será ativado após o pagamento!",
            _ => ""
        };
    }
}
