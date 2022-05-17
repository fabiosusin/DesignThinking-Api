namespace DTO.Integration.Surf.AccountDetails.Input
{
    public class  SurfAccountDetailsCpfInput
    {
        public SurfAccountDetailsCpfInput() { }
        public SurfAccountDetailsCpfInput(string cpf) => CPF = cpf;

        public string TransactionID { get; set; }
        public string CPF { get; set; }
    }
}
