namespace DTO.Integration.Asaas.Payments.Output
{
    public class AsaasGetQrCodeOutput
    {
        public bool Success { get; set; }
        public string EncodedImage { get; set; }
        public string Payload { get; set; }
    }
}
