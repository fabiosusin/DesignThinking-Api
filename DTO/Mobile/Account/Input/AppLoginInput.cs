namespace DTO.Mobile.Account.Input
{
    public class AppLoginInput
    {
        public AppLoginInput() { }
        public AppLoginInput(string mobileId, string allyId)
        {
            MobileId = mobileId;
            AllyId = allyId;
        }

        public string MobileId { get; set; }
        public string AllyId { get; set; }
    }
}
