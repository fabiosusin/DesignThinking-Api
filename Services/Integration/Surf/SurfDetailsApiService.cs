using DAO.DBConnection;
using DAO.Mobile.Surf;
using DTO.General.Api.Enum;
using DTO.Integration.Surf.AccountDetails.Input;
using DTO.Integration.Surf.AccountDetails.Output;
using DTO.Integration.Surf.AccountPlan.Input;
using DTO.Integration.Surf.AccountPlan.Output;
using DTO.Integration.Surf.BaseDetails.Output;
using DTO.Integration.Surf.Call.Input;
using DTO.Integration.Surf.Call.Output;
using DTO.Integration.Surf.SMS.Input;
using DTO.Integration.Surf.Subscriber.Input;
using DTO.Integration.Surf.Subscriber.Output;
using DTO.Integration.Surf.Token.Database;
using DTO.Integration.Surf.Token.Input;
using DTO.Integration.Surf.Token.Output;
using DTO.Integration.Surf.TopUp.Input;
using DTO.Integration.Surf.TopUp.Output;
using DTO.Surf.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Services.Integration.Surf
{
    internal class SurfDetailsApiService
    {
        protected SurfTokenInfoDAO TokenInfoDAO;
        private readonly ApiDispatcher _apiDispatcher;
        private const string BaseUrl = "https://www.pagtel.com.br/hub360/bitcom/";
        private const string BaseUrlV2 = BaseUrl + "v2/";

        public SurfDetailsApiService(XDataDatabaseSettings settings)
        {
            TokenInfoDAO = new(settings);
            _apiDispatcher = new ApiDispatcher();
        }

        public async Task<SurfAccountDetailsOutput> GetAccountDetails(SurfAccountDetailsInput input) => await SendRequest<SurfAccountDetailsOutput>(RequestMethodEnum.POST, $"{ BaseUrlV2 }/AccountDetails", input);
        public async Task<SurfAccountDetailsOutput> GetAccountDetailsByCPF(SurfAccountDetailsCpfInput input) => await SendRequest<SurfAccountDetailsOutput>(RequestMethodEnum.POST, $"{ BaseUrlV2 }/AccountDetailsCPF", input);
        public async Task<SurfCallHistoryOutput> GetCallHistory(SurfCallHistoryInput input) => await SendRequest<SurfCallHistoryOutput>(RequestMethodEnum.POST, $"{ BaseUrlV2 }/callHistory", input);
        public async Task<SurfBundleInfoOutput> GetBundleInfo(SurfBundleInfoInput input) => await SendRequest<SurfBundleInfoOutput>(RequestMethodEnum.POST, $"{ BaseUrlV2 }/BundleInfo", input);
        public async Task<SurfFreeUsageInfoOutput> GetFreeUsageInfo(SurfFreeUsageInfoInput input) => await SendRequest<SurfFreeUsageInfoOutput>(RequestMethodEnum.POST, $"{ BaseUrlV2 }/freeUsageInfo", input);
        public async Task<SurfSubscriberDetailsOutput> GetSubscriberInformation(SurfSubscriberDetailsInput input) => await SendRequest<SurfSubscriberDetailsOutput>(RequestMethodEnum.POST, $"{ BaseUrlV2 }/subscriberInformation", input);
        public async Task<SurfTopUpHistoryOutput> GetTopUpHistory(SurfTopUpHistoryInput input) => await SendRequest<SurfTopUpHistoryOutput>(RequestMethodEnum.POST, $"{ BaseUrlV2 }/topUpHistory", input);
        public async Task<SurfDetailsBaseApiOutput> SendSMS(SurfSendSmsOtpInput input) => await SendRequest<SurfDetailsBaseApiOutput>(RequestMethodEnum.POST, $"{ BaseUrlV2 }/SendSMS", input);
        public async Task<SurfDetailsBaseApiOutput> ValidateSMSToken(SurfValidateSmsOtpInput input) => await SendRequest<SurfDetailsBaseApiOutput>(RequestMethodEnum.POST, $"{ BaseUrlV2 }/ValidateSMSToken", input);

        private static Tuple<HttpRequestHeader, string>[] DefaultHeaders() => new List<Tuple<HttpRequestHeader, string>>
        {
            new Tuple<HttpRequestHeader, string>(HttpRequestHeader.Accept, "application/json"),
            new Tuple<HttpRequestHeader, string>(HttpRequestHeader.ContentType, "application/json"),
            new Tuple<HttpRequestHeader, string>(HttpRequestHeader.UserAgent, "XApi")
        }.ToArray();

        private static Tuple<string, string>[] V2Headers(string token) => new List<Tuple<string, string>>
        {
            new Tuple<string, string>("Authorization", $"Bearer { token }")
        }.ToArray();


        private async Task<T> SendRequest<T>(RequestMethodEnum method, string url, dynamic body = null)
        {
            var tries = 1;
        Retry:
            try
            {
                body.TransactionID = DateTime.Now.Ticks.ToString();
                var content = await _apiDispatcher.DispatchWithResponseUnDeserializeAsync(url, method, body, DefaultHeaders(), V2Headers(await GetToken().ConfigureAwait(false)));
                var baseOutput = JsonConvert.DeserializeObject<SurfDetailsBaseApiOutput>(content);
                if (baseOutput.Code == AppReturnCodesEnum.P04 && tries <= 3)
                    goto Retry;

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch { throw; }
        }

        private async Task<string> GetToken()
        {
            try
            {
                var tokenInfo = TokenInfoDAO.FindOne();
                if (!string.IsNullOrEmpty(tokenInfo?.Token) && tokenInfo.Expiration > DateTime.Now)
                    return tokenInfo.Token;

                var result = await TokenInfoOutput(tokenInfo?.RefreshToken).ConfigureAwait(false);
                if (result == null)
                    result = await TokenInfoOutput().ConfigureAwait(false);

                if (result.Message == "Falha ao autenticar")
                    result = await TokenInfoOutput().ConfigureAwait(false);

                if (!(result?.Authenticated ?? false))
                    throw new Exception(result?.Message ?? "Erro ao gerar o Token");

                var data = TokenInfoDAO.Upsert(new SurfTokenInfo
                {
                    Id = tokenInfo?.Id,
                    RefreshToken = result.RefreshToken,
                    Created = DateTime.Parse(result.Created),
                    Expiration = DateTime.Parse(result.Expiration),
                    Token = result.AccessToken
                })?.Data;

                return ((SurfTokenInfo)data)?.Token;
            }
            catch { throw; }
        }

        private async Task<SurfDetailsTokenInfoOutput> TokenInfoOutput(string refreshToken = null)
        {
            try
            {
                return await _apiDispatcher.DispatchWithResponseAsync<SurfDetailsTokenInfoOutput>(
                    $"{ BaseUrl }/api/login",
                    RequestMethodEnum.POST, string.IsNullOrEmpty(refreshToken) ?
                       new SurfDetailsGetTokenInput
                       {
                           Login = "apibitcom",
                           Password = "bitcom#120898@",
                           GrantType = "password"
                       } :
                       new SurfDetailsRefreshTokenInput
                       {
                           Login = "apibitcom",
                           RefreshToken = refreshToken,
                           GrantType = "refresh_token"
                       },
                       DefaultHeaders());
            }
            catch { throw; }
        }
    }
}
