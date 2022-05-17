using DTO.Integration.Surf.Token.Input;

namespace DTO.Integration.Surf.TopUp.Input
{
    public class SurfTopUpHistoryInput : SurfDetailsBaseApiInput
    {
        public SurfTopUpHistoryInput() { }
        public SurfTopUpHistoryInput(string msisdn, string month, string year) : base(msisdn)
        {
            Month = month;
            Year = year;
        }

        public string Year { get; set; }
        public string Month { get; set; }
    }
}
