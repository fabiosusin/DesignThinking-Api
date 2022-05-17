namespace DTO.Integration.Surf.Portability.Output
{
    public class SurfCheckPortabilityStatusOutput
    {
        public string Message { get; set; }
        public PortabilityStatusResult Payload { get; set; }
    }

    public class PortabilityStatusResult
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Msisdn { get; set; }
        public string Pmsisdn { get; set; }
        public string Cpf { get; set; }
        public string OperatorCode { get; set; }
        public string PortinDate { get; set; }
        public string Name { get; set; }
        public string MigrationWindow { get; set; }
        public string PortinFrom { get; set; }
        public string PortinTo { get; set; }
        public string Status { get; set; }
    }

}
