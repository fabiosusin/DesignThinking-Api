using DTO.General.Api.Enum;
using DTO.Integration.Asaas.Base.Output;
using DTO.Integration.Asaas.Customer.Input;
using DTO.Integration.Asaas.Customer.Output;
using DTO.Integration.Asaas.Payment.Output;
using DTO.Integration.Asaas.Payments.Input;
using DTO.Integration.Asaas.Payments.Output;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Useful.Extensions;
using Useful.Service;

namespace Services.Integration.Sige
{
    internal class AsaasApiService
    {
        private readonly ApiDispatcher _apiDispatcher;
        private static readonly string BaseUrl = $"{(EnvironmentService.Get() == EnvironmentService.Dev ? "https://sandbox.asaas.com" : "https://www.asaas.com")}/api/v3";
        private static readonly string ApiKey = EnvironmentService.Get() == EnvironmentService.Dev ? "148934f80cbcb57a6b15a291843f979c6ea805d47eae7d039f6f4d3590339852" : "8d1a7c01ab106e3736218788d82ab8552745343503821b7d5c0432934f2a5ad3";

        public AsaasApiService() => _apiDispatcher = new ApiDispatcher();

        public async Task<AsaasGetCustomerOutput> CreateCustomer(AsaasCreateCustomerInput input) => new(await SendRequest(RequestMethodEnum.POST, $"{ BaseUrl }/customers", input));

        public async Task<AsaasGetCustomerOutput> GetCustomer(string input) => new(await SendRequest(RequestMethodEnum.GET, $"{ BaseUrl }/customers/{input}"));

        public async Task<AsaasListCustomersResultOutput> ListCustomers(AsaasFilterCustomers input) => new(await SendRequest(RequestMethodEnum.GET, $"{ BaseUrl }/customers?{input?.GetQueryStringFromObject()}"));

        public async Task<AsaasCreateChargeResultOutput> CreateCharge(AsaasCreateChargeInput input) => new(await SendRequest(RequestMethodEnum.POST, $"{ BaseUrl }/payments", input));

        public async Task<AsaasCancelPaymentResultOutput> CancelCharge(string input) => new(await SendRequest(RequestMethodEnum.DELETE, $"{ BaseUrl }/payments/{input}"));

        public async Task<AsaasCreateChargeResultOutput> GetChargeDetails(string input) => new(await SendRequest(RequestMethodEnum.GET, $"{ BaseUrl }/payments/{input}"));

        public async Task<AsaasGetQrCodeResultOutput> GetQrCode(string input) => new(await SendRequest(RequestMethodEnum.GET, $"{ BaseUrl }/payments/{input}/pixQrCode"));

        private static Tuple<HttpRequestHeader, string>[] DefaultHeaders() => new Tuple<HttpRequestHeader, string>[]
            {
                new Tuple<HttpRequestHeader, string>(HttpRequestHeader.Accept, "application/json"),
                new Tuple<HttpRequestHeader, string>(HttpRequestHeader.ContentType, "application/json")
            };

        private static Tuple<string, string>[] AsaasHeaders() => new Tuple<string, string>[] { new Tuple<string, string>("access_token", ApiKey) };

        private async Task<AsaasRequestResultOutput> SendRequest(RequestMethodEnum method, string url, object body = null)
        {
            try
            {
                var resultJson = await _apiDispatcher.DispatchWithResponseUnDeserializeAsync(url, method, body, DefaultHeaders(), AsaasHeaders());
                var error = JsonConvert.DeserializeObject<AsaasDefaultErrorResult>(resultJson, _apiDispatcher._serializerSettings);
                return error?.Errors?.Any() ?? false ? new(error) : new(resultJson);
            }
            catch (Exception e)
            {
                var error = JsonConvert.DeserializeObject<AsaasDefaultErrorResult>(e.Message, _apiDispatcher._serializerSettings);
                if (error?.Errors?.Any() ?? false)
                    return new(error);

                throw;
            }
        }
    }
}
