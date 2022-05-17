using Business.API.Hub.Integration.Surf.Customer;
using DAO.DBConnection;
using DAO.General.Log;
using DAO.General.Surf;
using DAO.Hub.CustomerDAO;
using DAO.Mobile.Surf;
using DTO.External.Surf;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.General.Surf.Database;
using DTO.General.Surf.Input;
using DTO.Hub.Cellphone.Database;
using DTO.Integration.Surf.Recurrence.Input;
using DTO.Integration.Surf.Subscription.Output;
using Newtonsoft.Json;
using Services.Integration.Surf.Register.Recurrence;
using Services.Integration.Surf.Register.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API.External.Surf
{
    public class BlDeaflympics
    {
        private readonly LogHistoryDAO LogHistoryDAO;
        private readonly HubCustomerDAO HubCustomerDAO;
        private readonly BlSurfCustomer BlSurfCustomer;
        private readonly SurfMobilePlanDAO SurfMobilePlanDAO;
        private readonly SurfCustomerMsisdnDAO SurfCustomerMsisdnDAO;
        private readonly SurfRecurrenceService SurfRecurrenceService;
        private readonly SurfSubscriptionService SurfSubscriptionService;
        private readonly SurfDeaflympicsManagementDAO SurfDeaflympicsManagementDAO;
        public BlDeaflympics(XDataDatabaseSettings settings)
        {
            LogHistoryDAO = new(settings);
            HubCustomerDAO = new(settings);
            BlSurfCustomer = new(settings);
            SurfRecurrenceService = new();
            SurfSubscriptionService = new();
            SurfMobilePlanDAO = new(settings);
            SurfCustomerMsisdnDAO = new(settings);
            SurfDeaflympicsManagementDAO = new(settings);
        }

        public async Task<string> RegisterChips(SurfDeaflympicsRegisterChipsInput input)
        {
            if (!(input?.Iccids?.Any() ?? false))
                return "ICCID's não informados!";

            if (string.IsNullOrEmpty(input.MobilePlanId))
                return "MobilePlanId não informado!";

            if (string.IsNullOrEmpty(input.AllyId))
                return "AllyId não informado!";

            if (string.IsNullOrEmpty(input.CustomerId))
                return "CustomerId não informado!";

            if (string.IsNullOrEmpty(input.DDD) || input.DDD.Length != 2)
                return "DDD não informado corretamente!";

            var plan = SurfMobilePlanDAO.FindById(input.MobilePlanId);
            if (plan == null)
                return "Plano não encontrado!";

            if (string.IsNullOrEmpty(plan.SurfData?.Id))
                return "SurfData.Id não encontrado no Plano!";

            var surfCustomer = await BlSurfCustomer.GetSurfCustomer(input.CustomerId).ConfigureAwait(false);
            if (!(surfCustomer?.Success ?? false))
                return surfCustomer?.Message ?? "Não foi possível buscar o cliente na Surf!";

            if (string.IsNullOrEmpty(surfCustomer?.Code))
                return "Não foi possível buscar o código do cliente na Surf!";

            var result = new List<SurfDeaflympicsRegisterOutput>();
            foreach (var iccid in input.Iccids)
            {
                // Espera 1 segundo
                await Task.Delay(1000);

                var chipPayload = await GeneratePayload(new(iccid, plan.SurfData.Id, input.DDD), input.CustomerId, surfCustomer.Code).ConfigureAwait(false);
                if (!(chipPayload?.Success ?? false))
                    continue;

                var managementOutput = SurfDeaflympicsManagementDAO.Insert(new(input.AllyId, input.CustomerId, surfCustomer.Code, iccid, chipPayload.Id, plan, GetPayloadCellphoneData(chipPayload.Msisdn)));
                var management = (SurfDeaflympicsManagement)managementOutput.Data;

                _ = CancelRecurrence(new(management));
                _ = SurfCustomerMsisdnDAO.Insert(new(new(management)));

                result.Add(new(iccid, management.CellphoneData.DDD + management.CellphoneData.Number));
            }

            return result.Count == 0 ? "Nenhum chip pode ser ativado!" : string.Join(";", result.Select(x => $"{x.Iccid},{string.Format("{0:(##) #####-####}", long.Parse(x.Msisdn))}").ToArray()); 
        }

        private async Task<SurfSubscriptionContentOutput> GeneratePayload(SurfDeaflympicsManagamentInput input, string customerId, string surfCustomerCode)
        {
            if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(surfCustomerCode))
                return null;

            var customer = HubCustomerDAO.FindById(customerId);
            if (customer == null)
                return null;

            try
            {
                var payload = (await SurfSubscriptionService.AddSubscription(new(surfCustomerCode, new(customer, input))).ConfigureAwait(false))?.Payload?.FirstOrDefault();
                if (!(payload?.Success ?? false))
                {
                    RegisterLog(new(input.Iccid, payload?.Description ?? "Erro não identificado", "GeneratePayload", input));
                    return null;
                }

                return payload;
            }
            catch (Exception e)
            {
                RegisterLog(new(input.Iccid, "Erro ao Ativar Chip na Surf!", "GeneratePayload", e.Message, input));
                return null;
            }
        }

        private async Task CancelRecurrence(SurfRecurrenceInput input)
        {
            try
            {
                var result = await SurfRecurrenceService.AddRecurrence(input).ConfigureAwait(false);
                if (string.IsNullOrEmpty(result?.Payload?.Id))
                    RegisterLog(new(input.Iccid, "Erro ao cancelar a recorrência: " + result?.Message, "CancelRecurrence", input));
            }
            catch (Exception e)
            {
                RegisterLog(new(input.Iccid, "Erro adicionar recorrência na Surf!", "CancelRecurrence", e.Message, input));
            }
        }

        private static SurfCellphoneData GetPayloadCellphoneData(string msisdn) => string.IsNullOrEmpty(msisdn) ? null : new(msisdn);

        private void RegisterLog(SurfDeaflympicsRegisterLogInput input)
        {
            LogHistoryDAO.Insert(new AppLogHistory
            {
                Message = input.Message + $". ICCID: {input.IccId}",
                Type = AppLogTypeEnum.XApiSurfDeaflympicsRequestError,
                Method = input.Method,
                ExceptionMessage = input.ExceptionMessage,
                Data = input.Data != null ? JsonConvert.SerializeObject(input.Data) : null,
                Date = DateTime.Now
            });
        }
    }
}
