using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Intra.Game.Output
{
    public class IntraGameReportOutput : BaseApiOutput
    {
        public IntraGameReportOutput(string msg) : base(msg) { }
        public IntraGameReportOutput(List<IntraGameReportData> games) : base(true) => Games = games;
        public List<IntraGameReportData> Games { get; set; }
    }

    public class IntraGameReportData
    {
        public IntraGameReportData() { }
        public IntraGameReportData(string id, string name, string username)
        {
            PlayerId = id;
            PlayerName = name;
            Username = username;
        }
        public string Username { get; set; }
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int TotalGames { get; set; }
        public int TotalWins { get; set; }
        public int TotalLoses { get; set; }
        public int TotalShots { get; set; }
        public int TotalHits { get; set; }
        public int TotalErrors { get; set; }
        public decimal HitsPercentual { get; set; }
    }
}
