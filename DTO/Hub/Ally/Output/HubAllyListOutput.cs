using DTO.General.Base.Api.Output;
using DTO.Hub.Ally.Database;
using System.Collections.Generic;

namespace DTO.Hub.Ally.Output
{
    public class HubAllyListOutput : BaseApiOutput
    {
        public HubAllyListOutput(string msg) : base(msg) { }
        public HubAllyListOutput(IEnumerable<HubAlly> allys) : base(true) => Allys = allys;
        public IEnumerable<HubAlly> Allys { get; set; }
    }
}
