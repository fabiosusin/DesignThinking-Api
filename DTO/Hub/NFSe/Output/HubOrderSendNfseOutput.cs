using DTO.General.Base.Api.Output;
using DTO.Hub.Integration.NFSe.Output;
using DTO.Hub.Order.Enum;

namespace DTO.Hub.NFSe.Output
{
    public class HubOrderSendNfseOutput : BaseApiOutput
    {
        public HubOrderSendNfseOutput(string message) : base(message) { }
        public HubOrderSendNfseOutput(SendNfseOutput output) : base(true)
        {
            NfseOutput = output;
            Success = output?.Situation == HubNfseResultStatusEnum.Success;
            if (!Success)
                Message = output.Reason ?? "Resultado não informado!";
        }

        public SendNfseOutput NfseOutput { get; set; }
    }
}
