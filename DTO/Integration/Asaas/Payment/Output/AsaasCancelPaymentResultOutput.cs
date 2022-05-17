using DTO.Integration.Asaas.Base.Output;
using DTO.Integration.Asaas.Customer.Output;
using Newtonsoft.Json;

namespace DTO.Integration.Asaas.Payment.Output
{
    public class AsaasCancelPaymentResultOutput
    {
        public AsaasCancelPaymentResultOutput(AsaasCancelPaymentOutput charge) => Charge = charge;
        public AsaasCancelPaymentResultOutput(string errorMessage) => Error = new(errorMessage);
        public AsaasCancelPaymentResultOutput(AsaasRequestResultOutput result)
        {
            if (result == null)
                return;

            Error = result.Errors;

            if (string.IsNullOrEmpty(result.Json))
                return;

            var data = JsonConvert.DeserializeObject<AsaasCancelPaymentOutput>(result.Json);
            if (data != null)
                Charge = data;
        }


        public AsaasDefaultErrorResult Error { get; set; }
        public AsaasCancelPaymentOutput Charge { get; set; }
    }
}
