namespace DTO.Integration.Sige.Order.Output
{
    public class SigeOrderApiOutput
    {
        public SigeOrderApiOutput() { }
        public SigeOrderApiOutput(string message) => Mensagem = message;
        public SigeOrderApiOutput(bool scs) => Success = scs;
        public SigeOrderApiOutput(bool scs, string message)
        {
            Mensagem = message;
            Success = scs;
        }

        public string Mensagem { get; set; }
        public bool Success { get; set; }
        public SigeOrderOutput Pedido { get; set; }
    }
}
