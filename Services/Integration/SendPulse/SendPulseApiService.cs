using DAO.DBConnection;
using DAO.Integration.SendPulse;
using DTO.General.Api.Enum;
using DTO.Integration.Base.Input;
using DTO.Integration.SendPulse.Base.Output;
using DTO.Integration.SendPulse.SMS.Input;
using DTO.Integration.SendPulse.SMS.Output;
using DTO.Integration.SendPulse.Token.Input;
using DTO.Integration.SendPulse.Token.Output;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Services.Integration.SendPulse
{
    [Obsolete("Não será mais usado no momento, pois a solução não estava atendendo aos devidos requisitos: 22/02/2022")]
    internal class SendPulseApiService
    {
        private readonly ApiDispatcher _apiDispatcher;
        protected SendPulseTokenInfoDAO SendPulseTokenInfoDAO;
        private static readonly string BaseUrl = "https://api.sendpulse.com";
        private static readonly string ClientId = "068ce90036739177316623ce55ee1854";
        private static readonly string ClientSecret = "8ba3d52b559820fe2f42acd4e2c5af55";

        public SendPulseApiService(XDataDatabaseSettings settings)
        {
            _apiDispatcher = new ApiDispatcher();
            SendPulseTokenInfoDAO = new(settings);
        }

        public async Task<SendSmsResultOutput> SendSmsOutput(SendSmsInput input) => new(await SendRequest<SendSmsResultOutput>(RequestMethodEnum.POST, $"{ BaseUrl }/sms/send", input));

        private async Task<string> GetToken()
        {
            var tokenInfo = SendPulseTokenInfoDAO.FindOne();
            if (tokenInfo == null)
                tokenInfo = new();

            if (tokenInfo.Expiration < DateTime.Now)
            {
                var token = await TokenRequest(RequestMethodEnum.POST, $"{ BaseUrl }/oauth/access_token", new SendPulseGenerateTokenInput(GrantTypesInput.ClientCredentials, ClientId, ClientSecret));
                tokenInfo.Token = token.Token;
                tokenInfo.Expiration = DateTime.Now.AddSeconds(int.Parse(token.ExpiresIn));
                tokenInfo.Created = DateTime.Now;
                SendPulseTokenInfoDAO.Upsert(tokenInfo);
            }

            return tokenInfo.Token;
        }

        private static Tuple<HttpRequestHeader, string>[] DefaultHeaders(string token = null)
        {
            var result = new List<Tuple<HttpRequestHeader, string>> { new Tuple<HttpRequestHeader, string>(HttpRequestHeader.ContentType, "application/json") };

            if (!string.IsNullOrEmpty(token))
                result.Add(new Tuple<HttpRequestHeader, string>(HttpRequestHeader.Authorization, $"Bearer { token }"));

            return result.ToArray();
        }


        private async Task<SendPulseResultOutput> SendRequest<T>(RequestMethodEnum method, string url, object body = null)
        {
            try
            {
                var resultJson = await _apiDispatcher.DispatchWithResponseUnDeserializeAsync(url, method, body, DefaultHeaders(await GetToken().ConfigureAwait(false)));
                var error = JsonConvert.DeserializeObject<SendPulseApiError>(resultJson, _apiDispatcher._serializerSettings);
                return !string.IsNullOrEmpty(error?.Message) ? new(error) : new(resultJson);
            }
            catch
            {
                throw;
            }
        }

        private async Task<SendPulseTokenOutput> TokenRequest(RequestMethodEnum method, string url, object body)
        {
            try { return await _apiDispatcher.DispatchWithResponseAsync<SendPulseTokenOutput>(url, method, body, DefaultHeaders()); }
            catch { throw; }
        }
    }
}
