using DTO.Integration.Asaas.Base.Output;
using DTO.Integration.Asaas.Customer.Output;
using Newtonsoft.Json;

namespace DTO.Integration.Asaas.Payments.Output
{
    public class AsaasCreateChargeResultOutput
    {
        public AsaasCreateChargeResultOutput(AsaasCreateChargeOutput payment) => Charge = payment;
        public AsaasCreateChargeResultOutput(string errorMessage) => Error = new(errorMessage);
        public AsaasCreateChargeResultOutput(AsaasRequestResultOutput result)
        {
            if (result == null)
                return;

            Error = result.Errors;

            if (string.IsNullOrEmpty(result.Json))
                return;

            var data = JsonConvert.DeserializeObject<AsaasCreateChargeOutput>(result.Json);
            if (data != null)
                Charge = data;
        }


        public AsaasDefaultErrorResult Error { get; set; }
        public AsaasCreateChargeOutput Charge { get; set; }
    }
}
