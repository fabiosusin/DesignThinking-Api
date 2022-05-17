using System.Net;
using System.Threading.Tasks;
using DTO.General.Address.Output;
using DTO.General.Api.Enum;

namespace Services.Integration.ViaCep
{
    internal class ViaCepApiService
    {
        private readonly ApiDispatcher _apiDispatcher;
        private static readonly string BaseUrl = "https://viacep.com.br/ws";

        public ViaCepApiService()
        {
            _apiDispatcher = new ApiDispatcher();
        }

        public async Task<ViaCepAddressOutput> GetAddress(string zipcode) =>
            await SendRequest<ViaCepAddressOutput>(RequestMethodEnum.GET, $"{ BaseUrl }/{WebUtility.UrlEncode(zipcode)}/json/");


        private async Task<T> SendRequest<T>(RequestMethodEnum method, string url, object body = null)
        {
            try
            {
                return await _apiDispatcher.DispatchWithResponseAsync<T>(url, method, body);
            }
            catch
            {
                throw;
            }
        }
    }
}
