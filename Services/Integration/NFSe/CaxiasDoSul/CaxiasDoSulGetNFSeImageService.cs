using DTO.Hub.Integration.NFSe.Input;
using DTO.Hub.Integration.NFSe.Output;
using System.Text;
using System.Threading.Tasks;

namespace Services.Integration.NFSe.CaxiasDoSul
{
    public class CaxiasDoSulGetNFSeImageService
    {
        internal CaxiasDoSulNFSeApiService CaxiasDoSulApiService;
        public CaxiasDoSulGetNFSeImageService() => CaxiasDoSulApiService = new();

        public async Task<GetNfseImageOutput> GetNfseImage(GetNfseDetailsInput input)
        {
            if (input?.Certified == null)
                return null;

            var signXml = new StringBuilder();

            signXml.Append("<pedidoNFSePNG versao=\"1.0\">");

            signXml.Append($"<CNPJ>{input.Cnpj}</CNPJ>");
            signXml.Append($"<chvAcessoNFS-e>{input.AccessKey}</chvAcessoNFS-e>");
            signXml.Append("</pedidoNFSePNG>");

            var xml = new StringBuilder();
            xml.Append("<obterNotasEmPNG>");
            xml.Append(GenerateXmlSign.SignXML(input.Certified, signXml.ToString()));
            xml.Append("</obterNotasEmPNG>");

            return await CaxiasDoSulApiService.SendNfseRequest<GetNfseImageOutput>(xml.ToString()).ConfigureAwait(false);
        }
    }
}
