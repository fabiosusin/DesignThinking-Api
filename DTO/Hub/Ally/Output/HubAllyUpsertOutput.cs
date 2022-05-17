using DTO.General.Base.Api.Output;

namespace DTO.Hub.Ally.Output
{
    public class HubAllyUpsertOutput : BaseApiOutput
    {
        public HubAllyUpsertOutput(string msg) : base(msg) { }
        public HubAllyUpsertOutput(bool scs, string id) : base(scs) => AllyId = id;
        public string AllyId { get; set; }
    }
}
