using Business.API.Hub.Integration.Sige.Order;
using DAO.DBConnection;
using DAO.General.Log;
using DAO.Hub.Cellphone;
using DTO.Integration.Surf.Recurrence.Output;
using DTO.Hub.Cellphone.Database;
using Newtonsoft.Json;
using Services.Integration.Surf.Register.Recurrence;
using System;
using System.Linq;
using System.Threading.Tasks;
using DTO.General.Base.Api.Output;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Hub.Cellphone.Enum;
using DTO.Hub.Order.Enum;
using Business.API.Hub.Integration.Asaas.Customer;
using DTO.Integration.Surf.Recurrence.Enum;
using DTO.General.Invoice.Database;
using Business.API.Hub.Customer;

namespace Business.API.Hub.Integration.Surf.Recurrence
{
    public class BlSurfRecurrence
    {
        private readonly BlSigeOrder BlSigeOrder;
        private readonly LogHistoryDAO LogHistoryDAO;
        private readonly BlAsaasCharge BlAsaasCharge;
        private readonly BlInvoiceCustomer BlInvoiceCustomer;
        private readonly SurfRecurrenceService SurfRecurrenceService;
        private readonly HubCellphoneManagementDAO HubCellphoneManagementDAO;
        private readonly HubCellphoneManagementRecurrenceDAO HubCellphoneManagementRecurrenceDAO;
        public BlSurfRecurrence(XDataDatabaseSettings settings)
        {
            BlSigeOrder = new(settings);
            BlAsaasCharge = new(settings);
            LogHistoryDAO = new(settings);
            SurfRecurrenceService = new();
            BlInvoiceCustomer = new(settings);
            HubCellphoneManagementDAO = new(settings);
            HubCellphoneManagementRecurrenceDAO = new(settings);
        }

        public async Task RegisterRecurrence(DateTime input)
        {
            _ = GenerateInvoiceRecurrence(input);
            var managements = HubCellphoneManagementDAO.GetRecurrence(input);
            if (!(managements?.Any() ?? false))
                return;

            foreach (var management in managements)
            {
                if (management.Status == HubCellphoneManagementStatusEnum.AwaitingChargePayment)
                {
                    management.Status = HubCellphoneManagementStatusEnum.Defaulter;
                    _ = BlInvoiceCustomer.SendSmsToCustomer(management.Id, "Sua fatura vence hoje, realize o pagamento antes que seu plano seja cancelado");
                    HubCellphoneManagementDAO.Update(management);
                }

                var result = management.Status switch
                {
                    HubCellphoneManagementStatusEnum.Completed => await ChangeRecurrenceStatus(management.Id, SurfRecurrenceStatus.Paid).ConfigureAwait(false),
                    HubCellphoneManagementStatusEnum.Defaulter => await ChangeRecurrenceStatus(management.Id, SurfRecurrenceStatus.Defaulter).ConfigureAwait(false),
                    _ => new(true)
                };

                if (!(result?.Success ?? false))
                {
                    LogHistoryDAO.Insert(new AppLogHistory
                    {
                        Message = "Erro ao gerar recorrência!",
                        Type = AppLogTypeEnum.XApiHubValidationError,
                        Method = "RegisterRecurrence",
                        Data = JsonConvert.SerializeObject(result),
                        Date = DateTime.Now
                    });

                    continue;
                }

                management.RecurrenceDate = DateTime.Now.AddDays(30);
                management.LastUpdate = DateTime.Now;
                HubCellphoneManagementDAO.Update(management);

                if (management.Status != HubCellphoneManagementStatusEnum.Completed)
                    continue;

                var sigeOrderResult = await BlSigeOrder.CreateRecurrenceSigeOrder(management.OrderId).ConfigureAwait(false);
                if (sigeOrderResult?.Success ?? false)
                    continue;

                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = "Geraçao Venda ERP: " + (sigeOrderResult?.Mensagem ?? "Não foi possível realizar a venda no ERP."),
                    Type = AppLogTypeEnum.XApiHubValidationError,
                    Method = "RegisterRecurrence",
                    Data = JsonConvert.SerializeObject(sigeOrderResult),
                    Date = DateTime.Now
                });
            }
        }

        private async Task<BaseApiOutput> GenerateInvoiceRecurrence(DateTime date)
        {
            var managements = HubCellphoneManagementDAO.GetDateRecurrence(date.AddDays(10));
            if (!(managements?.Any() ?? false))
                return new("Não existe cobrança para a data informada");

            var result = new BaseApiOutput(true);
            foreach (var management in managements)
            {
                var resultRecurrence = await ChangeRecurrenceStatus(management.Id, SurfRecurrenceStatus.AwaitingChargePayment).ConfigureAwait(false);
                if (!resultRecurrence.Success)
                {
                    LogHistoryDAO.Insert(new AppLogHistory
                    {
                        Message = resultRecurrence.Message,
                        Type = AppLogTypeEnum.XApiSurfRequestError,
                        Method = "GenerateInvoiceRecurrence",
                        Data = JsonConvert.SerializeObject(management),
                        Date = DateTime.Now
                    });

                    result.Message = resultRecurrence.Message;
                    continue;
                }

                var chargeResult = await BlAsaasCharge.CreateCharge(management.OrderId, new(HubOrderPaymentFormEnum.Pix, management.Price?.OrderPrice ?? 0)).ConfigureAwait(false);
                if (chargeResult == null || (chargeResult.Error?.Errors?.Any() ?? false))
                {
                    var errorMessage = string.Join('.', chargeResult.Error?.Errors?.Select(x => x.Description)) ?? $"Não foi possível gerar a cobrança de recorrência, para o gerênciamento {management.Id}!";

                    LogHistoryDAO.Insert(new AppLogHistory
                    {
                        Message = errorMessage,
                        Type = AppLogTypeEnum.XApiAsaasRequestError,
                        Method = "GenerateInvoiceRecurrence",
                        Data = chargeResult != null ? JsonConvert.SerializeObject(errorMessage) : null,
                        Date = DateTime.Now
                    });

                    result.Message = errorMessage;
                }
                else
                {
                    var invoice = new InvoiceCustomer(chargeResult, management.CustomerId, management.Id, management.AllyId);
                    var resultInsert = await BlInvoiceCustomer.SaveInvoice(invoice).ConfigureAwait(false);

                    if (!resultInsert.Success)
                    {
                        LogHistoryDAO.Insert(new AppLogHistory
                        {
                            Message = resultInsert.Message,
                            Type = AppLogTypeEnum.XApiAsaasRequestError,
                            Method = "GenerateInvoiceRecurrence",
                            Data = invoice != null ? JsonConvert.SerializeObject(invoice) : null,
                            Date = DateTime.Now
                        });
                    }
                }
            }

            return string.IsNullOrEmpty(result.Message) && result.Success ? new(true) : new(result.Message);
        }

        public async Task<BaseApiOutput> ChangeRecurrenceStatus(string managementId, SurfRecurrenceStatus type)
        {
            if (string.IsNullOrEmpty(managementId))
                return new("Gerenciamento Telefônico não informado!");

            var surfRecurrenceResult = await GenerateSurfReport(managementId, type).ConfigureAwait(false);
            return string.IsNullOrEmpty(surfRecurrenceResult?.Payload?.Id) ? new("Erro ao gerar recorrência: " + surfRecurrenceResult?.Message) : new(true);
        }

        private async Task<SurfRecurrenceOutput> GenerateSurfReport(string managementId, SurfRecurrenceStatus type)
        {
            try
            {
                var management = HubCellphoneManagementDAO.FindById(managementId);
                if (management == null)
                    return null;

                var result = await SurfRecurrenceService.AddRecurrence(new(management, type == SurfRecurrenceStatus.Paid)).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(result?.Payload?.Id))
                {
                    management.Status = HubCellphoneManagement.GetStatusFromSurfReportStatus(type);
                    HubCellphoneManagementDAO.Update(management);
                    HubCellphoneManagementRecurrenceDAO.Insert(new(managementId, result?.Payload?.Id, HubCellphoneManagementRecurrence.GetStatusFromSurfReportStatus(type)));
                }

                return result;
            }
            catch (Exception e)
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = "Erro adicionar recorrência na Surf!",
                    Type = AppLogTypeEnum.XApiSurfRequestError,
                    ExceptionMessage = e.Message,
                    Method = "GenerateSurfReport",
                    Data = managementId,
                    Date = DateTime.Now
                });
                return null;
            }
        }
    }
}
