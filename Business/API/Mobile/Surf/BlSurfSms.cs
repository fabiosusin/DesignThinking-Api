using DAO.DBConnection;
using DTO.Integration.Surf.BaseDetails.Output;
using DTO.Integration.Surf.SMS.Input;
using DTO.Surf.Enum;
using Services.Integration.Surf.Register.Customer;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Business.API.Mobile.Surf
{
    public class BlSurfSms
    {
        private SurfSMSService SurfSMSService;
        public BlSurfSms(XDataDatabaseSettings settings)
        {
            SurfSMSService = new(settings);
        }

        public async Task<SurfDetailsBaseApiOutput> SendSms(SurfSendSmsOtpInput input)
        {
            if (input == null)
                return new SurfDetailsBaseApiOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "Requisição mal formada!"
                };

            if (string.IsNullOrEmpty(input.MSISDN))
                return new SurfDetailsBaseApiOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "MSISDN não informado!"
                };

            if (input.TokenValidate && string.IsNullOrEmpty(input.Text))
                input.Text = "Seu Código de Verificação de Acesso ao Aplicativo é:";

            if (string.IsNullOrEmpty(input.Text))
                return new SurfDetailsBaseApiOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "Nenhum texto para o SMS informado!"
                };

            return await SurfSMSService.SendSMS(input).ConfigureAwait(false);
        }

        public async Task<SurfDetailsBaseApiOutput> ValidateSmsOpt(SurfValidateSmsOtpInput input)
        {
            if (input == null)
                return new SurfDetailsBaseApiOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "Requisição mal formada!"
                };

            if (string.IsNullOrEmpty(input.MSISDN))
                return new SurfDetailsBaseApiOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "MSISDN não informado!"
                };

            if (string.IsNullOrEmpty(input.Value))
                return new SurfDetailsBaseApiOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "Código não informado!"
                };

            return await SurfSMSService.ValidateSMSToken(input).ConfigureAwait(false);
        }
    }
}
