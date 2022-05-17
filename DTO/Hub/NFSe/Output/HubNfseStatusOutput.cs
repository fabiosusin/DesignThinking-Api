using DTO.General.Base.Api.Output;
using DTO.Hub.Integration.NFSe.Output;
using DTO.Hub.Order.Enum;

namespace DTO.Hub.NFSe.Output
{
    public class HubNfseStatusOutput : BaseApiOutput
    {
        public HubNfseStatusOutput(string message) : base(message) { }
        public HubNfseStatusOutput(GetNfseStatusOutput output) : base(true)
        {
            NfseOutput = output;
            Success = output?.Nfse?.Sit == HubNfseResultStatusEnum.Success;
            if (!Success)
                Message = output.Nfse.Reasons.Mot ?? "Resultado não informado!";
        }

        public GetNfseStatusOutput NfseOutput { get; set; }
    }
}
