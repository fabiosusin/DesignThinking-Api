using DTO.General.Base.Api.Output;
using DTO.Hub.Integration.NFSe.Output;
using DTO.Hub.Order.Enum;

namespace DTO.Hub.NFSe.Output
{
    public class HubNfseImageOutput : BaseApiOutput
    {
        public HubNfseImageOutput(string message) : base(message) { }
        public HubNfseImageOutput(GetNfseImageOutput output) : base(true)
        {
            NfseOutput = output;
            Success = output?.Situation == HubNfseResultStatusEnum.Success;
            if (!Success)
                Message = "Resultado não informado!";
        }

        public GetNfseImageOutput NfseOutput { get; set; }
    }
}