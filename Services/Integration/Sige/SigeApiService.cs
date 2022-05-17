using DTO.General.Api.Enum;
using DTO.General.Base.Api.Output;
using DTO.Integration.Sige.AccountPlan.Input;
using DTO.Integration.Sige.AccountPlan.Output;
using DTO.Integration.Sige.Customer.Input;
using DTO.Integration.Sige.Customer.Output;
using DTO.Integration.Sige.Entry.Input;
using DTO.Integration.Sige.Order.Input;
using DTO.Integration.Sige.Order.Output;
using DTO.Integration.Sige.Product.Output;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Services.Integration.Sige
{
    internal class SigeApiService
    {
        private readonly ApiDispatcher _apiDispatcher;
        private static readonly string BaseUrl = "https://api.sigecloud.com.br";

        public SigeApiService()
        {
            _apiDispatcher = new ApiDispatcher();
        }

        public async Task<List<SigeAccountPlanOutput>> GetAccountPlans(SigeAccountPlanFiltersInput input) =>
            await SendRequest<List<SigeAccountPlanOutput>>(RequestMethodEnum.GET, $"{ BaseUrl }/request/PlanosConta/Pesquisar?{input?.GetQueryStringFromObject()}");

        public async Task<List<SigeCustomerOutput>> GetCustomer(SigeCustomerFiltersInput input) =>
            await SendRequest<List<SigeCustomerOutput>>(RequestMethodEnum.GET, $"{ BaseUrl }/request/Pessoas/Pesquisar?{input?.GetQueryStringFromObject()}");

        public async Task<BaseApiOutput> CreateEntry(SigeEntryInput input)
        {
            try
            {
                _ = await SendRequestUnDeserialized(RequestMethodEnum.POST, $"{ BaseUrl }/request/Lancamentos/Criar", input);
                return new(true);
            }
            catch (Exception ex)
            {
                return new(ex?.Message ?? "Não foi possível realizar a requisição.");
            }
        }

        public async Task<BaseApiOutput> UpdateCustomer(SigeCustomerInput input)
        {
            try
            {
                var resultJson = await SendRequestUnDeserialized(RequestMethodEnum.POST, $"{ BaseUrl }/request/Pessoas/Salvar", input);
                var resultMessage = string.IsNullOrEmpty(resultJson) ? "Não foi possível realizar a requisição." : Regex.Replace(resultJson, @"[&\/\\#,+()$~%.'"":*?<>{}]", string.Empty);
                return new(resultMessage.ToLower().Contains("sucesso"), resultMessage);
            }
            catch (Exception ex)
            {
                return new(ex?.Message ?? "Não foi possível realizar a requisição.");
            }
        }

        public async Task<SigeOrderApiOutput> CreateOrder(SigeOrderInput input)
        {
            var resultJson = await SendRequestUnDeserialized(RequestMethodEnum.POST, $"{ BaseUrl }/request/Pedidos/SalvarEFaturar?retornarPedido=true", input);
            if (!string.IsNullOrEmpty(resultJson))
                resultJson = resultJson[1..^1].Replace("\\", "").Replace("\"\"", "\"");

            try
            {
                var result = JsonConvert.DeserializeObject<SigeOrderApiOutput>(resultJson, _apiDispatcher._serializerSettings);
                result.Success = result.Mensagem?.ToLower()?.Contains("sucesso") ?? false;
                return result;
            }
            catch
            {
                return new SigeOrderApiOutput(false, resultJson ?? "Não foi possível realizar a requisição.");
            }
        }

        public async Task<IEnumerable<SigeProductInput>> GetProducts() =>
            await SendRequest<IEnumerable<SigeProductInput>>(RequestMethodEnum.GET, $"{ BaseUrl }/request/Produtos/GetAll?pageSize=0&skip=0");

        private static Tuple<HttpRequestHeader, string>[] DefaultHeaders() => new Tuple<HttpRequestHeader, string>[]
            {
                new Tuple<HttpRequestHeader, string>(HttpRequestHeader.Accept, "application/json"),
                new Tuple<HttpRequestHeader, string>(HttpRequestHeader.ContentType, "application/json")
            };

        private static Tuple<string, string>[] SigeHeaders() => new Tuple<string, string>[] {
                new Tuple<string, string>("Authorization-Token", "90ecda4b14976e0a261bef9718a81e4c05425cc1c3fbb76eaff6b6dfe673eedf3b5bf08c3adf734ccc8a4208d9aa2cf212a2d055e6091744c4603b786e3ad3bfe092a6bc8bc5455102718231b4ffbfc221393d76e89116e17b99fa036ebfd9845a2237b430b4641ba903be538ed357c05a6399720c965b1be7649881182fe876"),
                new Tuple<string, string>("User", "software@xplay.digital"),
                new Tuple<string, string>("App", "XPlayPDV")
            };

        private async Task<T> SendRequest<T>(RequestMethodEnum method, string url, object body = null)
        {
            try
            {
                return await _apiDispatcher.DispatchWithResponseAsync<T>(url, method, body, DefaultHeaders(), SigeHeaders());
            }
            catch
            {
                throw;
            }
        }

        private async Task<string> SendRequestUnDeserialized(RequestMethodEnum method, string url, object body = null)
        {
            try
            {
                return await _apiDispatcher.DispatchWithResponseUnDeserializeAsync(url, method, body, DefaultHeaders(), SigeHeaders());
            }
            catch
            {
                throw;
            }
        }
    }
}
