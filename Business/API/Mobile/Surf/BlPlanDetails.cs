using DAO.DBConnection;
using DAO.Mobile.Surf;
using DTO.Integration.Surf.AccountPlan.Input;
using DTO.Integration.Surf.AccountPlan.Output;
using DTO.Surf.Enum;
using DTO.Surf.Output.AccountPlan;
using Services.Integration.Surf.Register.Customer;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Business.API.Mobile.Surf
{
    public class BlPlanDetails
    {
        private SurfPlanService SurfPlanService;
        private SurfMobilePlanDAO SurfMobilePlanDAO;
        public BlPlanDetails(XDataDatabaseSettings settings)
        {
            SurfPlanService = new(settings);
            SurfMobilePlanDAO = new(settings);
        }

        public async Task<SurfBundleInfoOutput> BundleInfo(string msisdn) => string.IsNullOrEmpty(msisdn) ?
            new SurfBundleInfoOutput
            {
                CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                Msg = "MSISDN não informado!"
            } : await SurfPlanService.GetBundleInfo(new SurfBundleInfoInput(msisdn)).ConfigureAwait(false);

        public async Task<SurfFreeUsageInfoOutput> FreeUsageInfo(string msisdn, string bundleCode)
        {
            if (string.IsNullOrEmpty(msisdn))
                return new SurfFreeUsageInfoOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "MSISDN não informado!"
                };

            return await SurfPlanService.GetFreeUsageInfo(new SurfFreeUsageInfoInput(msisdn, bundleCode)).ConfigureAwait(false);
        }

        public async Task<AppSurfMobilePlanOutput> GetAccountPlanInfo(string msisdn)
        {
            if (string.IsNullOrEmpty(msisdn))
                return new AppSurfMobilePlanOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "MSISDN não informado!"
                };

            var freeUsageInfo = await FreeUsageInfo(msisdn, "").ConfigureAwait(false);
            if (freeUsageInfo == null)
                return new AppSurfMobilePlanOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "Informações de uso não encontrados!"
                };

            var account = SurfMobilePlanDAO.FindOne(x => x.Name == freeUsageInfo.BundleName);
            if (account == null)
                return new AppSurfMobilePlanOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "Informações da conta não encontradas!"
                };

            _ = decimal.TryParse(freeUsageInfo.FreeData?.Split('.')?[0], out var data);
            _ = decimal.TryParse(freeUsageInfo.FreeNetMinutes, out var netMinutes);
            _ = decimal.TryParse(freeUsageInfo.FreeSMSMinutes, out var smsMinutes);

            var result = new AppSurfMobilePlanOutput(account.Name)
            {
                Internet = new AccountData
                {
                    Limit = account.InternetLimit,
                    ToUse = data
                },
                Call = new AccountData
                {
                    Limit = account.CallLimit,
                    ToUse = netMinutes
                },
                Sms = new AccountData
                {
                    Limit = account.SmsLimit,
                    ToUse = smsMinutes
                },
            };

            return result;
        }
    }
}
