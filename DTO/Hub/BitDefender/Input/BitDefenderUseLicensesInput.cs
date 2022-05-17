namespace DTO.Hub.BitDefender.Input
{
    public class BitDefenderUseLicensesInput
    {
        public BitDefenderUseLicensesInput(string id, int quantity)
        {
            CategoryId = id;
            Quantity = quantity;
        }
        public string CategoryId { get; set; }
        public int Quantity { get; set; }
    }
}
