using DTO.Hub.Order.Input;

namespace DTO.Integration.Sige.Order.Input
{
    public class SigeProductOrderInput
    {
        public SigeProductOrderInput(HubProductOrderInput input)
        {
            if (input == null)
                return;

            Codigo = input.Code;
            Descricao = input.Name;
            Quantidade = input.Quantity;
            ValorUnitario = input.Price?.Price ?? 0m;
        }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
