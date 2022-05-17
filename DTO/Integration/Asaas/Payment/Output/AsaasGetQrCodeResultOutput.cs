using DTO.Integration.Asaas.Base.Output;
using DTO.Integration.Asaas.Customer.Output;
using Newtonsoft.Json;

namespace DTO.Integration.Asaas.Payments.Output
{
    public class AsaasGetQrCodeResultOutput
    {
        public AsaasGetQrCodeResultOutput(AsaasGetQrCodeOutput payment) => QrCode = payment;
        public AsaasGetQrCodeResultOutput(string errorMessage) => Error = new(errorMessage);
        public AsaasGetQrCodeResultOutput(AsaasRequestResultOutput result)
        {
            if (result == null)
                return;

            Error = result.Errors;

            if (string.IsNullOrEmpty(result.Json))
                return;

            var data = JsonConvert.DeserializeObject<AsaasGetQrCodeOutput>(result.Json);
            if (data != null)
            {
                if (data.Success)
                    QrCode = data;
                else if (Error == null)
                    Error = new("Não foi possível gerar o QR Code");
            }
        }


        public AsaasDefaultErrorResult Error { get; set; }
        public AsaasGetQrCodeOutput QrCode { get; set; }
    }
}
