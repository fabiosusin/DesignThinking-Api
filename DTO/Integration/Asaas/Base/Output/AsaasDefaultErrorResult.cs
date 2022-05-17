using System.Collections.Generic;

namespace DTO.Integration.Asaas.Base.Output
{
    public class AsaasDefaultErrorResult
    {
        public AsaasDefaultErrorResult() { }
        public AsaasDefaultErrorResult(string message) => Errors = new List<AsaasError> { new(message) };

        public List<AsaasError> Errors { get; set; }
    }

    public class AsaasError
    {
        public AsaasError() { }
        public AsaasError(string message) => Description = message;

        public string Code { get; set; }
        public string Description { get; set; }
    }
}
