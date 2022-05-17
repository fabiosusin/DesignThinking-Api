using DTO.General.Api.Enum;
using DTO.Integration.Base.Input;
using DTO.Integration.Surf.Customer.Output;
using DTO.Integration.Surf.Input.Customer;
using DTO.Integration.Surf.Operator.Output;
using DTO.Integration.Surf.Plan.Output;
using DTO.Integration.Surf.Portability.Input;
using DTO.Integration.Surf.Portability.Output;
using DTO.Integration.Surf.Recurrence.Input;
using DTO.Integration.Surf.Recurrence.Output;
using DTO.Integration.Surf.Subscription.Input;
using DTO.Integration.Surf.Subscription.Output;
using DTO.Integration.Surf.Token.Input;
using DTO.Integration.Surf.Token.Output;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Integration.Surf
{
    internal class SurfApiService
    {
        private readonly ApiDispatcher _apiDispatcher;
        private static SurfTokenOutput SurfTokenOutput;
        private static readonly string BaseUrl = "https://www.pagtel.com.br/isp/api";
        private static readonly string AccessUsername = "35bc0ac0-1abe-11eb-adc1-0242ac120002";
        private static readonly string AccessPassword = "09e74770-fdcf-4404-a136-b9566ba4ecf3";

        public SurfApiService() => _apiDispatcher = new ApiDispatcher();

        public async Task<SurfOperatorOutput> GetOperators() =>
            await SendRequest<SurfOperatorOutput>(RequestMethodEnum.GET, $"{ BaseUrl }/v1/operators");

        public async Task<SurfPlanOutput> GetPlans() =>
            await SendRequest<SurfPlanOutput>(RequestMethodEnum.GET, $"{ BaseUrl }/v1/plans");

        public async Task<SurfPortabilityOutput> AddPortability(SurfPortabalityInput input) =>
            await SendRequest<SurfPortabilityOutput>(RequestMethodEnum.POST, $"{ BaseUrl }/v1/portabilities", input);

        public async Task<SurfCheckPortabilityStatusOutput> ChekPortabilityStatus(SurfCheckPortabilityStatusInput input) =>
            await SendRequest<SurfCheckPortabilityStatusOutput>(RequestMethodEnum.POST, $"{ BaseUrl }/v1/portabilities/check-status", input);

        public async Task<SurfResendPortabilitySmsOutput> ResendPortabilitySms(SurfResendPortabilitySmsInput input) =>
            await SendRequest<SurfResendPortabilitySmsOutput>(RequestMethodEnum.POST, $"{ BaseUrl }/v1/portabilities/sms", input);

        public async Task<SurfRecurrenceOutput> AddRecurrence(SurfRecurrenceInput input) =>
            await SendRequest<SurfRecurrenceOutput>(RequestMethodEnum.POST, $"{ BaseUrl }/v1/subscriptions/reports", input);

        public async Task<SurfCustomerOutput> GetCustomer(string input) =>
            await SendRequest<SurfCustomerOutput>(RequestMethodEnum.GET, $"{ BaseUrl }/v1/customers/{input}");

        public async Task<SurfCustomerOutput> AddCustomer(SurfCustomerInput input) =>
            await SendRequest<SurfCustomerOutput>(RequestMethodEnum.POST, $"{ BaseUrl }/v1/customers", input);

        public async Task<SurfSubscriptionOutput> AddSubscription(SurfSubscriptionInput input) =>
            await SendRequest<SurfSubscriptionOutput>(RequestMethodEnum.POST, $"{ BaseUrl }/v1/subscriptions", input);

        private static Tuple<HttpRequestHeader, string>[] DefaultHeaders()
        {
            var result = new List<Tuple<HttpRequestHeader, string>>
               {
                new Tuple<HttpRequestHeader, string>(HttpRequestHeader.Accept, "application/json"),
                new Tuple<HttpRequestHeader, string>(HttpRequestHeader.ContentType, "application/json")
               };

            if (!string.IsNullOrEmpty(SurfTokenOutput?.Token))
                result.Add(new Tuple<HttpRequestHeader, string>(HttpRequestHeader.Authorization, $"Bearer { SurfTokenOutput.Token }"));

            return result.ToArray();
        }

        private async Task<T> SendRequest<T>(RequestMethodEnum method, string url, object body = null)
        {
            try
            {
                await GenerateToken().ConfigureAwait(false);
                return await _apiDispatcher.DispatchWithResponseAsync<T>(url, method, body, DefaultHeaders());
            }
            catch
            {
                throw;
            }
        }

        private async Task GenerateToken()
        {
            if (SurfTokenOutput != null)
                return;

            SurfTokenOutput = await TokenRequest(RequestMethodEnum.POST, $"{ BaseUrl }/v1/token", new SurfGenerateTokenInput(GrantTypesInput.ClientCredentials));
        }

        private async Task<SurfTokenOutput> TokenRequest(RequestMethodEnum method, string url, object body)
        {
            try
            {
                return await _apiDispatcher.DispatchWithResponseAsync<SurfTokenOutput>(url, method, body, DefaultHeaders(), new Tuple<string, string>[]
                {
                    new Tuple<string, string>("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{AccessUsername}:{AccessPassword}")))
                });
            }
            catch
            {
                throw;
            }
        }
    }
}
