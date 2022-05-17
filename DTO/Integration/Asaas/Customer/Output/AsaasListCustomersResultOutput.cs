using DTO.Integration.Asaas.Base.Output;
using Newtonsoft.Json;

namespace DTO.Integration.Asaas.Customer.Output
{
    public class AsaasListCustomersResultOutput
    {
        public AsaasListCustomersResultOutput(string errorMessage) => Error = new(errorMessage);

        public AsaasListCustomersResultOutput(AsaasRequestResultOutput result)
        {
            if (result == null)
                return;

            Error = result.Errors;

            if (string.IsNullOrEmpty(result.Json))
                return;

            var data = JsonConvert.DeserializeObject<AsaasListCustomersOutput>(result.Json);
            if (data != null)
                Customers = data;
        }

        public AsaasDefaultErrorResult Error { get; set; }
        public AsaasListCustomersOutput Customers { get; set; }
    }
}
