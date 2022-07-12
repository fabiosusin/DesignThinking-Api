using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Intra.Game.Output
{
    public class IntraGameListOutput : BaseApiOutput
    {
        public IntraGameListOutput(string msg) : base(msg) { }
        public IntraGameListOutput(List<IntraGameOutput> games) : base(true) => Games = games;
        public List<IntraGameOutput> Games { get; set; }
    }

    public class IntraGameOutput
    {
        public string Username { get; set; }
        public string PlayerOneName { get; set; }
        public string PlayerTwoName { get; set; }
        public string Scoreboard { get; set; }
    }
}
