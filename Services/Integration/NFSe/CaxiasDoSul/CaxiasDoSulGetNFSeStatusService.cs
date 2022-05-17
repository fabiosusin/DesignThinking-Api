using DTO.Hub.Integration.NFSe.Input;
using DTO.Hub.Integration.NFSe.Output;
using System.Text;
using System.Threading.Tasks;

namespace Services.Integration.NFSe.CaxiasDoSul
{
    public class CaxiasDoSulGetNFSeStatusService
    {
        internal CaxiasDoSulNFSeApiService CaxiasDoSulApiService;
        public CaxiasDoSulGetNFSeStatusService() => CaxiasDoSulApiService = new();

        public async Task<GetNfseStatusOutput> GetNfseStatus(GetNfseStatusInput input)
        {
            if (input?.Certified == null)
                return null;

            var signXml = new StringBuilder();

            signXml.Append("<pedidoStatusLote versao=\"1.0\">");

            signXml.Append($"<CNPJ>{input.Cnpj}</CNPJ>");
            signXml.Append($"<cLote>{input.Lot}</cLote>");
            signXml.Append("</pedidoStatusLote>");

            var xml = new StringBuilder();
            xml.Append("<obterCriticaLote>");
            xml.Append(GenerateXmlSign.SignXML(input.Certified, signXml.ToString()));
            xml.Append("</obterCriticaLote>");

            return await CaxiasDoSulApiService.SendNfseRequest<GetNfseStatusOutput>(xml.ToString()).ConfigureAwait(false);
        }
    }
}
