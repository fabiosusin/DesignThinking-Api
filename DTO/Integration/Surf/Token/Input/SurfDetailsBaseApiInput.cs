namespace DTO.Integration.Surf.Token.Input
{
    public class SurfDetailsBaseApiInput
    {
        public SurfDetailsBaseApiInput() { }
        public SurfDetailsBaseApiInput(string msisdn) => MSISDN = msisdn;
        public string TransactionID { get; set; }
        public string MSISDN { get; set; }
    }
}
