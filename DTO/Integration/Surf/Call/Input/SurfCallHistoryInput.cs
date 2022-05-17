using DTO.Integration.Surf.Token.Input;

namespace DTO.Integration.Surf.Call.Input
{
    public class SurfCallHistoryInput : SurfDetailsBaseApiInput
    {
        public SurfCallHistoryInput() { }
        public SurfCallHistoryInput(string msisdn, string month, string year) : base(msisdn)
        {
            Month = month;
            Year = year;
        }

        public string Year { get; set; }
        public string Month { get; set; }
    }
}
