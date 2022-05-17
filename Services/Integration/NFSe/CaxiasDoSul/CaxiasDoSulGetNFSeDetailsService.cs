using DTO.Hub.Integration.NFSe.Input;
using DTO.Hub.Integration.NFSe.Output;
using System.Text;
using System.Threading.Tasks;

namespace Services.Integration.NFSe.CaxiasDoSul
{
    public class CaxiasDoSulGetNFSeDetailsService
    {
        internal CaxiasDoSulNFSeApiService CaxiasDoSulApiService;
        public CaxiasDoSulGetNFSeDetailsService() => CaxiasDoSulApiService = new();

        public async Task<GetNfseDetailsOutput> GetNfseDetails(GetNfseDetailsInput input)
        {
            if (input?.Certified == null)
                return null;

            var signXml = new StringBuilder();

            signXml.Append("<pedidoNFSe versao=\"1.0\">");

            signXml.Append($"<CNPJ>{input.Cnpj}</CNPJ>");
            signXml.Append($"<chvAcessoNFS-e>{input.AccessKey}</chvAcessoNFS-e>");
            signXml.Append("</pedidoNFSe>");

            var xml = new StringBuilder();
            xml.Append("<obterNotaFiscal>");
            xml.Append(GenerateXmlSign.SignXML(input.Certified, signXml.ToString()));
            xml.Append("</obterNotaFiscal>");

            return await CaxiasDoSulApiService.SendNfseRequest<GetNfseDetailsOutput>(xml.ToString()).ConfigureAwait(false);
        }
    }
}
