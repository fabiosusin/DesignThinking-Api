using DTO.Integration.Asaas.Base.Output;

namespace DTO.Integration.Asaas.Customer.Output
{
    public class AsaasRequestResultOutput
    {
        public AsaasRequestResultOutput(AsaasDefaultErrorResult errors) => Errors = errors;
        public AsaasRequestResultOutput(string json) => Json = json;

        public AsaasDefaultErrorResult Errors { get; set; }
        public string Json { get; set; }
    }
}
