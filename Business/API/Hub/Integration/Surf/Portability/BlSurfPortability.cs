using DAO.DBConnection;
using DAO.General.Log;
using DAO.Hub.Cellphone;
using DAO.Hub.CustomerDAO;
using DTO.General.Base.Api.Output;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Hub.Cellphone.Database;
using DTO.Hub.Cellphone.Input;
using DTO.Hub.Integration.Surf.Enum;
using DTO.Integration.Surf.Portability.Input;
using DTO.Integration.Surf.Portability.Output;
using Newtonsoft.Json;
using Services.Integration.Surf.Register.Portability;
using System;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Business.API.Hub.Integration.Surf.Portability
{
    public class BlSurfPortability
    {
        protected HubCellphoneManagementDAO HubCellphoneManagementDAO;
        protected SurfPortabilityService SurfPortabilityService;
        protected HubCustomerDAO HubCustomerDAO;
        protected LogHistoryDAO LogHistoryDAO;

        public BlSurfPortability(XDataDatabaseSettings settings)
        {
            LogHistoryDAO = new(settings);
            HubCustomerDAO = new(settings);
            HubCellphoneManagementDAO = new(settings);
            SurfPortabilityService = new();
        }

        public async Task<BaseApiOutput> ResendSms(HubCellphonePortabilityInput input) => await BasePortabilityAction(input, HubSurfPortabilityActionEnum.ResendSms).ConfigureAwait(false);

        public async Task<BaseApiOutput> CheckStatus(HubCellphonePortabilityInput input) => await BasePortabilityAction(input, HubSurfPortabilityActionEnum.CheckStatus).ConfigureAwait(false);

        public async Task<SurfPortabilityOutput> GeneratePortability(HubCellphoneManagement management, CellphoneManagementPortability portability, string customerId, string payloadId)
        {
            if (management == null || string.IsNullOrEmpty(portability?.OperatorId) || string.IsNullOrEmpty(portability.Number) || string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(payloadId))
                return null;

            var customer = HubCustomerDAO.FindById(customerId);
            if (customer == null)
                return null;

            try
            {
                portability.Number = portability.Number.GetMsisdn();
                return await SurfPortabilityService.AddPortability(new(portability, customer, payloadId)).ConfigureAwait(false);
            }
            catch { return null; }
        }

        private async Task<BaseApiOutput> BasePortabilityAction(HubCellphonePortabilityInput input, HubSurfPortabilityActionEnum type)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (type == HubSurfPortabilityActionEnum.Unknown)
                return new("Ação não informada!");

            if (string.IsNullOrEmpty(input.ManagementId))
                return new("Gerenciamento não informado!");

            var management = HubCellphoneManagementDAO.FindById(input.ManagementId);
            if (management == null)
                return new("Gerenciamento Telefônico não encontrado!");

            if (management.AllyId != input.AllyId)
                return new("Gerenciamento Telefônico não pertence a este aliado!");

            if (management.CellphoneData == null)
                return new("Dados da linha não encontrados!");

            var customer = HubCustomerDAO.FindById(management.CustomerId);
            if (customer == null)
                return new("Cliente não encontrado!");

            switch (type)
            {
                case HubSurfPortabilityActionEnum.CheckStatus:
                    var msisdnStatus = management.CellphoneData.CountryPrefix + management.CellphoneData.DDD + management.CellphoneData.Number;
                    var checkStatusOutput = await CheckPortabilityStatus(new(msisdnStatus, input.OriginalNumber, customer.Document?.Data)).ConfigureAwait(false);
                    return checkStatusOutput?.Payload == null ? new("Ocorreu um erro ao reenviar o SMS!") : new(true, checkStatusOutput.Payload.Description);
                
                case HubSurfPortabilityActionEnum.ResendSms:
                    var msisdnResendSms = management.CellphoneData.CountryPrefix + management.CellphoneData.DDD + management.CellphoneData.Number;
                    var sendSmsOutput = await ResendSmsAsync(new(msisdnResendSms, input.OriginalNumber, customer.Document?.Data)).ConfigureAwait(false);
                    if (sendSmsOutput?.Payload == null)
                        return new("Ocorreu um erro ao reenviar o SMS!");
                    break;
            }

            return new(true);
        }

        private async Task<SurfResendPortabilitySmsOutput> ResendSmsAsync(SurfResendPortabilitySmsInput input)
        {
            try
            {
                return await SurfPortabilityService.ResendPortabilitySms(input).ConfigureAwait(false);
            }
            catch { return null; }
        }

        private async Task<SurfCheckPortabilityStatusOutput> CheckPortabilityStatus(SurfCheckPortabilityStatusInput input)
        {
            try
            {
                return await SurfPortabilityService.CheckPortabilityStatus(input).ConfigureAwait(false);
            }
            catch { return null; }
        }
    }
}
