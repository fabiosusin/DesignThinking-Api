using DAO.DBConnection;
using DTO.Integration.Surf.TopUp.Input;
using DTO.Integration.Surf.TopUp.Output;
using DTO.Surf.Enum;
using Services.Integration.Surf.Register.Customer;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Business.API.Mobile.Surf
{
    public class BlTopUp
    {
        private SurfTopUpService SurfTopUpService;
        public BlTopUp(XDataDatabaseSettings settings)
        {
            SurfTopUpService = new(settings);
        }
        public async Task<SurfTopUpHistoryOutput> TopUpHistory(SurfTopUpHistoryInput input) => input == null ?
            new SurfTopUpHistoryOutput
            {
                CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                Msg = "Requisição mal formada!"
            } : string.IsNullOrEmpty(input.MSISDN) ? new SurfTopUpHistoryOutput
            {
                CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                Msg = "MSISDN não informado!"
            } : await SurfTopUpService.GetTopUpHistory(input).ConfigureAwait(false);
    }
}
