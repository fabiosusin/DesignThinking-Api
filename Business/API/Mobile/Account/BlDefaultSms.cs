using Business.API.Mobile.Surf;
using DAO.DBConnection;
using DAO.General.Log;
using DAO.General.Surf;
using DAO.Mobile.Account;
using DTO.General.Base.Api.Output;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Integration.SendPulse.SMS.Input;
using DTO.Mobile.Account.Database;
using DTO.Mobile.Account.Enum;
using DTO.Mobile.Account.Input;
using DTO.Surf.Enum;
using Services.Integration.Facilita.Sms;
using System;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Business.API.Mobile.Account
{
    public class BlDefaultSms
    {
        private readonly BlSurfSms BlSurfSms;
        private readonly AppOtpCodeDAO AppOtpCodeDAO;
        private readonly LogHistoryDAO LogHistoryDAO;
        private readonly BlCustomerMsisdn BlCustomerMsisdn;
        private readonly FacilitaSendSmsService FacilitaSendSmsService;
        public BlDefaultSms(XDataDatabaseSettings settings)
        {
            BlSurfSms = new(settings);
            AppOtpCodeDAO = new(settings);
            LogHistoryDAO = new(settings);
            BlCustomerMsisdn = new(settings);
            FacilitaSendSmsService = new();
        }

        // Usuário de Teste XPlay, não deve mandar SMS e sempre deverá ser usado o código abaixo para liberar no App
        private const string MasterMobileId = "99012345678";
        private const string MasterMobileCode = "1973";

        public async Task<BaseApiOutput> SendSms(AppSendSmsInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.MobileId))
                return new("MobileId não informado!");

            if (input.MobileId == MasterMobileId)
                return new(true);

            if (!input.MobileId.CellphoneIsValid())
                return new("MobileId não informado corretamente!");

            if (string.IsNullOrEmpty(input.Sender))
                return new("Remetente não informado!");

            if (string.IsNullOrEmpty(input.Body))
                return new("Corpo de mensagem não informado!");

            if (input.Type == AppSmsTypeEnum.Unknown)
                return new("Tipo de SMS não informado!");

            var surfSms = await SendSurfSms(input).ConfigureAwait(false);
            if (surfSms.Success)
                return new(true);

            if (input.Type == AppSmsTypeEnum.Otp)
            {
                AppOtpCodeDAO.DisableAllUserCodes(input.MobileId);
                var code = NumberExtension.RandomNumber(1000, 9999).ToString();

                input.Body += $" {code}";
                AppOtpCodeDAO.Insert(new AppOtpCode(input.MobileId, code, DateTime.Now.AddMinutes(10)));
            }

            var result = await FacilitaSendSmsService.SendSms(new SendSmsInput(input.Sender, input.Body, input.MobileId)).ConfigureAwait(false);
            return !(result?.Success ?? false) ? new(result?.Message ?? "Não foi possível enviar o SMS") : new(true);
        }

        public async Task<BaseApiOutput> ValidateSmsOtp(AppValidateOtpInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.MobileId))
                return new("UserId não informado!");

            if (string.IsNullOrEmpty(input.Code))
                return new("Código não informado!");

            if (input.MobileId == MasterMobileId)
            {
                if (input.Code != MasterMobileCode)
                    return new("Código não encontrado!");

                return new(true);
            }

            var surfResult = await ValidateSurfSms(input).ConfigureAwait(false);
            if (surfResult.Success)
                return new(true);

            var code = AppOtpCodeDAO.FindOne(x => x.AppMobileId == input.MobileId && x.Code == input.Code);
            if (code == null)
                return new("Código não encontrado!");

            if (code.Expiration < DateTime.Now)
                return new("Código expirado!");

            if (code.Used)
                return new("Código já utilizado!");

            code.Used = true;
            AppOtpCodeDAO.Update(code);

            return new(true);
        }

        private async Task<BaseApiOutput> SendSurfSms(AppSendSmsInput input)
        {
            var msisdn = BlCustomerMsisdn.GetMsisdn(input.MobileId);
            if (msisdn == null)
                return new(false);

            try
            {
                var surfResult = await BlSurfSms.SendSms(new(input, msisdn.NumberCountryPrefix + msisdn.Number)).ConfigureAwait(false);
                if (surfResult?.Code == AppReturnCodesEnum.P00)
                    return new(true);
            }
            catch (Exception e)
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    ExceptionMessage = e.Message,
                    Message = "Erro ao SMS na Surf!",
                    Type = AppLogTypeEnum.XApiSurfRequestError,
                    Method = "SendSurfSms",
                    Date = DateTime.Now
                });
            }

            return new(false);
        }

        private async Task<BaseApiOutput> ValidateSurfSms(AppValidateOtpInput input)
        {
            var msisdn = BlCustomerMsisdn.GetMsisdn(input.MobileId);
            if (msisdn == null)
                return new(false);

            try
            {
                var surfResult = await BlSurfSms.ValidateSmsOpt(new(input, msisdn.NumberCountryPrefix + msisdn.Number)).ConfigureAwait(false);
                if (surfResult?.Code == AppReturnCodesEnum.P00)
                    return new(true);
            }
            catch (Exception e)
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    ExceptionMessage = e.Message,
                    Message = "Erro ao validar SMS na Surf!",
                    Type = AppLogTypeEnum.XApiSurfRequestError,
                    Method = "ValidateSurfSms",
                    Date = DateTime.Now
                });
            }

            return new(false);
        }       
    } 
}
