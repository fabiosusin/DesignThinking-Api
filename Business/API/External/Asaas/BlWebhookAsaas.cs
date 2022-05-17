using Business.API.Hub.Entry;
using Business.API.Hub.Integration.Surf.Base;
using Business.API.Hub.Integration.Surf.Recurrence;
using Business.API.Hub.Order;
using DAO.DBConnection;
using DAO.General.Invoice;
using DAO.General.Log;
using DAO.Hub.Cellphone;
using DAO.Hub.Order;
using DTO.External.Asaas.Input;
using DTO.General.Invoice.Database;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Hub.Order.Database;
using DTO.Hub.Order.Enum;
using DTO.Integration.Surf.Recurrence.Enum;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API.External.Asaas
{
    public class BlWebhookAsaas
    {
        private readonly BlEntry BlEntry;
        private readonly BlOrder BlOrder;
        private readonly BlBaseSurf BlBaseSurf;
        private readonly HubOrderDAO HubOrderDAO;
        private readonly LogHistoryDAO LogHistoryDAO;
        private readonly BlSurfRecurrence BlSurfRecurrence;
        private readonly InvoiceCustomerDAO InvoiceCustomerDAO;
        private readonly HubPaymentOrderDAO HubPaymentOrderDAO;
        private readonly HubCellphoneManagementDAO HubCellphoneManagementDAO;
        public BlWebhookAsaas(XDataDatabaseSettings settings)
        {
            BlOrder = new(settings);
            BlEntry = new(settings);
            BlBaseSurf = new(settings);
            HubOrderDAO = new(settings);
            LogHistoryDAO = new(settings);
            BlSurfRecurrence = new(settings);
            HubPaymentOrderDAO = new(settings);
            InvoiceCustomerDAO = new(settings);
            HubCellphoneManagementDAO = new(settings);
        }

        public void UpdateChargeAsync(AsaasUpdateInput input)
        {
            if (string.IsNullOrEmpty(input?.Payment?.Id))
            {
                SendErrorLog("Pagamento estava num formato inválido!", input);
                return;
            }

            var payment = HubPaymentOrderDAO.FindOne(x => x.AsaasData.AsaasId == input.Payment.Id);
            var invoice = InvoiceCustomerDAO.FindOne(x => x.AsaasId == input.Payment.Id);

            if (payment == null && invoice == null)
            {
                SendErrorLog("Não existe Pagamento e nenhuma Invoice para o Id: " + input.Payment.Id, input);
                return;
            }

            if (payment != null)
                _ = UpdateOrderPayment(input, payment);

            if (invoice != null)
                UpdateInvoiceCustomer(input, invoice);
        }

        private async void UpdateInvoiceCustomer(AsaasUpdateInput input, InvoiceCustomer invoice)
        {
            if (invoice == null)
            {
                SendErrorLog("Invoice não encontrada para o Id: " + input.Payment.Id, input);
                return;
            }

            var management = HubCellphoneManagementDAO.FindById(invoice.CellphoneManagementId);
            if (management == null)
            {
                SendErrorLog("Gerênciamento Telefônico não encontrado para o Id: " + invoice.CellphoneManagementId, invoice);
                return;
            }

            var paymentStatus = HubPaymentOrder.GetStatusFromAsaasStatusStr(input.Payment.Status);
            if (paymentStatus == HubPaymentOrderType.Unknown)
            {
                SendErrorLog("Status de cobrança não encontrado para o Id: " + input.Payment.Id, input);
                return;
            }

            invoice.Paid = paymentStatus == HubPaymentOrderType.Paid;
            if (!invoice.Paid)
                return;

            invoice.PayIn = DateTime.Now;
            InvoiceCustomerDAO.Update(invoice);

            var result = await BlSurfRecurrence.ChangeRecurrenceStatus(invoice.CellphoneManagementId, SurfRecurrenceStatus.Paid).ConfigureAwait(false);
            if (!result.Success)
                SendErrorLog(result.Message, invoice);
        }

        private async Task UpdateOrderPayment(AsaasUpdateInput input, HubPaymentOrder payment)
        {
            if (payment == null)
            {
                SendErrorLog("Pagamento não encontrado para o Id: " + input.Payment.Id, input);
                return;
            }

            var paymentStatus = HubPaymentOrder.GetStatusFromAsaasStatusStr(input.Payment.Status);
            if (paymentStatus == HubPaymentOrderType.Unknown)
            {
                SendErrorLog("Status de cobrança não encontrado para o Id: " + input.Payment.Id, input);
                return;
            }

            payment.Status = paymentStatus;
            HubPaymentOrderDAO.Update(payment);

            if (paymentStatus != HubPaymentOrderType.Paid)
                return;

            var order = HubOrderDAO.FindById(payment.OrderId);
            if (order == null)
            {
                SendErrorLog("Venda não encontrada para o pagamento Id: " + payment.Id, payment);
                return;
            }

            var orderPayments = HubPaymentOrderDAO.FindByOrderId(order.Id);
            if (!(orderPayments?.Any() ?? false))
            {
                SendErrorLog("Não foi possível encontrar os pagamentos para a venda Id: " + order.Id, order, "Erro ao buscar Pagamentos!");
                return;
            }

            if (orderPayments.Any(x => x.Status != HubPaymentOrderType.Paid))
                return;

            HubOrderDAO.UpdateOrderStatus(order.Id, HubOrderStatusEnum.Paid);
            var sendCellphonesToPaid = BlBaseSurf.CellphonePaymentCofirmed(order.Id);
            if (!sendCellphonesToPaid.Success)
            {
                SendErrorLog(sendCellphonesToPaid.Message, orderPayments, "Erro ao atualizar Gerenciamento Telefônico!");
                return;
            }

            var celphones = HubCellphoneManagementDAO.FindByOrderId(order.Id);
            foreach (var cellphone in celphones)
            {
                var result = await BlBaseSurf.ReprocessCellphoneManagement(cellphone.Id, order.AllyId).ConfigureAwait(false);
                if (!result.Success)
                    SendErrorLog(result.ErrorMessage, cellphone, "Erro ao ativar o chip");
            }
        }

        private void SendErrorLog(string message, object input, string title = "Erro ao atualizar Cobrança Asaas!")
        {
            LogHistoryDAO.Insert(new AppLogHistory
            {
                Message = title,
                ExceptionMessage = message,
                Type = AppLogTypeEnum.XApiAsaasRequestError,
                Data = JsonConvert.SerializeObject(input),
                Method = "UpdateCharge",
                Date = DateTime.Now
            });
        }
    }
}
