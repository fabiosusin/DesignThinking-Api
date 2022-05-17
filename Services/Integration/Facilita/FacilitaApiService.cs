using DTO.General.Api.Enum;
using DTO.Integration.Facilita.SMS.Input;
using DTO.Integration.Facilita.SMS.Output;
using DTO.Integration.SendPulse.SMS.Input;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Services.Integration.Facilita
{
    internal class FacilitaApiService
    {
        private readonly ApiDispatcher _apiDispatcher;
        private static readonly string BaseUrl = "http://api.facilitamovel.com.br/api";
        private static readonly string User = "xplaydigital";
        private static readonly string Password = "Xsms2021!";

        public FacilitaApiService() => _apiDispatcher = new ApiDispatcher();

        public async Task<FacilitaSendSmsResultOutput> SendSmsOutput(SendSmsInput input) => new(await SendRequest(RequestMethodEnum.GET, $"{ BaseUrl }/simpleSend.ft?user={User}&password={Password}&{new FacilitaSendSmsInput(input).GetQueryStringFromObject()}"));

        private static Tuple<HttpRequestHeader, string>[] DefaultHeaders(string token = null)
        {
            var result = new List<Tuple<HttpRequestHeader, string>> { new Tuple<HttpRequestHeader, string>(HttpRequestHeader.ContentType, "application/json") };

            if (!string.IsNullOrEmpty(token))
                result.Add(new Tuple<HttpRequestHeader, string>(HttpRequestHeader.Authorization, $"Bearer { token }"));

            return result.ToArray();
        }


        private async Task<string> SendRequest(RequestMethodEnum method, string url, object body = null)
        {
            try { return await _apiDispatcher.DispatchWithResponseUnDeserializeAsync(url, method, body, DefaultHeaders()); } catch { throw; }
        }
    }
}
