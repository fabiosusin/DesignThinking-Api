using DTO.General.Base.Api.Output;

namespace DTO.Hub.Cellphone.Output
{
    public class HubSurfFindCustomerOutput : BaseApiOutput
    {
        public HubSurfFindCustomerOutput(string msg) : base(msg) { }
        public HubSurfFindCustomerOutput(bool success, string code) : base(success) => Code = code;

        public string Code { get; set; }
    }
}
