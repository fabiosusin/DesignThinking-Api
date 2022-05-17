namespace DTO.Integration.Sige.Order.Input
{
    public class SigePaymentOrderInput
    {
        public SigePaymentOrderInput(string paymentForm, decimal val, bool quitar)
        {
            FormaPagamento = paymentForm;
            ValorPagamento = val;
            Quitar = quitar;
        }

        public string FormaPagamento { get; set; }
        public decimal ValorPagamento { get; set; }
        public bool Quitar { get; set; }
    }
}
