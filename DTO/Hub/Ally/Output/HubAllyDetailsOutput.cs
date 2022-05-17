using DTO.General.Base.Api.Output;
using DTO.Hub.Ally.Database;

namespace DTO.Hub.Ally.Output
{
    public class HubAllyDetailsOutput : BaseApiOutput
    {
        public HubAllyDetailsOutput(string msg) : base(false, msg) { }
        public HubAllyDetailsOutput(HubAlly input) : base(input != null)
        {
            if (input == null)
                return;

            Ally = input;
        }
        public HubAlly Ally { get; set; }
    }
}
