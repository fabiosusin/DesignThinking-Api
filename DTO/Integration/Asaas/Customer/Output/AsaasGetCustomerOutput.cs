using DTO.Integration.Asaas.Base.Output;
using Newtonsoft.Json;

namespace DTO.Integration.Asaas.Customer.Output
{
    public class AsaasGetCustomerOutput
    {
        public AsaasGetCustomerOutput(string errorMessage) => Error = new(errorMessage);
        public AsaasGetCustomerOutput(AsaasRequestResultOutput result)
        {
            if (result == null)
                return;

            Error = result.Errors;

            if (string.IsNullOrEmpty(result.Json))
                return;

            var data = JsonConvert.DeserializeObject<AsaasCreateCustomerOutput>(result.Json);
            if (data != null)
                Customer = data;
        }

        public AsaasDefaultErrorResult Error { get; set; }
        public AsaasCreateCustomerOutput Customer { get; set; }
    }
}
