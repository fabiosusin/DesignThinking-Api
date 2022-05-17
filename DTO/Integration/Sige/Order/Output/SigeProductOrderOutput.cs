namespace DTO.Integration.Sige.Order.Output
{
    public class SigeProductOrderOutput
    {
        public string Codigo { get; set; }
        public object Unidade { get; set; }
        public string Descricao { get; set; }
        public long Quantidade { get; set; }
        public double ValorUnitario { get; set; }
        public long ValorFrete { get; set; }
        public long DescontoUnitario { get; set; }
        public long ValorTotal { get; set; }
        public long PesoKg { get; set; }
        public long Comprimento { get; set; }
        public long Altura { get; set; }
        public long Largura { get; set; }
        public bool FreteGratis { get; set; }
        public long ValorUnitarioFrete { get; set; }
        public long PrazoEntregaFrete { get; set; }
        public long ComissaoVendedor { get; set; }
        public long Seguro { get; set; }
        public object Composicoes { get; set; }
        public object Atributos { get; set; }
    }
}
