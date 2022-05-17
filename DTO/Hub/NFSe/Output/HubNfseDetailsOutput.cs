using DTO.General.Base.Api.Output;
using DTO.Hub.Integration.NFSe.Output;
using DTO.Hub.Order.Enum;

namespace DTO.Hub.NFSe.Output
{
    public class HubNfseDetailsOutput : BaseApiOutput
    {
        public HubNfseDetailsOutput(string message) : base(message) { }
        public HubNfseDetailsOutput(GetNfseDetailsOutput output) : base(true)
        {
            NfseOutput = output;
            Success = output?.Situation == HubNfseResultStatusEnum.Success;
            if (!Success)
                Message = "Resultado não informado!";
        }

        public GetNfseDetailsOutput NfseOutput { get; set; }
    }
}