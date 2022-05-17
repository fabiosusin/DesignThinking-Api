using DTO.Hub.Integration.NFSe.Input;
using DTO.Hub.Integration.NFSe.Output;
using System;
using System.Text;
using System.Threading.Tasks;
using Useful.Extensions;
using Useful.Service;

namespace Services.Integration.NFSe.CaxiasDoSul
{
    public class CaxiasDoSulSendNFSeService
    {
        internal CaxiasDoSulNFSeApiService CaxiasDoSulApiService;
        public CaxiasDoSulSendNFSeService() => CaxiasDoSulApiService = new();

        public async Task<SendNfseOutput> SendNfse(SendNfseInput nfse)
        {
            if (nfse?.Provider == null)
                return null;

            var signXml = new StringBuilder();

            signXml.Append("<envioLote versao=\"1.0\">");

            signXml.Append($"<CNPJ>{nfse.Provider.Cnpj}</CNPJ>");
            signXml.Append($"<dhTrans>{DateTime.Now:yyyy-MM-dd HH:MM:ss}</dhTrans>");

            signXml.Append("<NFS-e>");
            signXml.Append("<infNFSe versao=\"1.1\">");

            signXml.Append(GetIdTag(nfse));
            signXml.Append(GetPrestTag(nfse.Provider));
            signXml.Append(GetTomSTag(nfse.Customer));
            signXml.Append(GetDetTag(nfse));
            signXml.Append(GetTotalTag(nfse.TotalPrice));

            signXml.Append("<infAdicLT>4305108</infAdicLT>");
            signXml.Append("</infNFSe>");
            signXml.Append("</NFS-e>");
            signXml.Append("</envioLote>");

            var xml = new StringBuilder();
            xml.Append("<enviarLoteNotas>");
            xml.Append(GenerateXmlSign.SignXML(nfse.Certified, signXml.ToString()));
            xml.Append("</enviarLoteNotas>");

            return await CaxiasDoSulApiService.SendNfseRequest<SendNfseOutput>(xml.ToString()).ConfigureAwait(false);
        }

        private static string GetIdTag(SendNfseInput nfse)
        {
            if (nfse == null)
                return string.Empty;

            var xml = new StringBuilder();
            xml.Append("<Id>");

            xml.Append($"<cNFS-e>{NumberExtension.RandomNumber(9)}</cNFS-e>");
            xml.Append("<mod>90</mod>");
            xml.Append("<serie>S</serie>");
            xml.Append($"<nNFS-e>{nfse.Number}</nNFS-e>");
            xml.Append($"<dEmi>{DateTime.Now:yyyy-MM-dd}</dEmi>");
            xml.Append($"<hEmi>{DateTime.Now:HH:mm}</hEmi>");
            xml.Append("<tpNF>1</tpNF>");
            xml.Append($"<refNF>{GetRefNF(nfse)}</refNF>");
            xml.Append($"<ambienteEmi>{(EnvironmentService.Get() == EnvironmentService.Dev ? "2" : "1")}</ambienteEmi>");
            xml.Append("<formaEmi>2</formaEmi>");
            xml.Append("<empreitadaGlobal>2</empreitadaGlobal>");

            xml.Append("</Id>");
            return xml.ToString();
        }

        private static string GetPrestTag(NfseServiceProviderInput provider)
        {
            if (provider == null)
                return string.Empty;

            var xml = new StringBuilder();
            xml.Append("<prest>");

            xml.Append($"<CNPJ>{provider.Cnpj}</CNPJ>");
            xml.Append($"<xNome>{provider.Name}</xNome>");

            xml.Append($"<IM>{provider.Im}</IM>");
            xml.Append($"<xEmail>{provider.Email}</xEmail>");
            xml.Append($"<xSite>{provider.Site}</xSite>");

            if (provider.Address?.IsValid() ?? false)
            {
                xml.Append("<end>");
                xml.Append($"<xLgr>{provider.Address.Street}</xLgr>");
                xml.Append($"<nro>{provider.Address.Number}</nro>");
                xml.Append($"<xBairro>{provider.Address.Neighborhood}</xBairro>");
                xml.Append("<cMun>4305108</cMun>");
                xml.Append($"<xMun>{provider.Address.City}</xMun>");
                xml.Append($"<UF>{provider.Address.State}</UF>");
                xml.Append($"<CEP>{provider.Address.ZipCode}</CEP>");
                xml.Append("<cPais>01058</cPais>");
                xml.Append($"<xPais>{provider.Address.Country}</xPais>");
                xml.Append("</end>");
            }

            xml.Append($"<regimeTrib>{provider.TaxRegime}</regimeTrib>");
            xml.Append("</prest>");
            return xml.ToString();
        }

        private static string GetTomSTag(NfseCustomerInput customer)
        {
            if (customer == null)
                return string.Empty;

            var xml = new StringBuilder();
            xml.Append("<TomS>");
            var cpfCnpj = customer.CpfCnpj.FormatWhitouthCharacters();
            xml.Append(cpfCnpj.Length == 11 ? $"<CPF>{cpfCnpj}</CPF>" : $"<CNPJ>{cpfCnpj}</CNPJ>");
            xml.Append($"<xNome>{customer.Name}</xNome>");
            xml.Append("<ender>");

            if (customer.Address?.IsValid() ?? false)
            {
                xml.Append($"<xLgr>{customer.Address.Street}</xLgr>");
                xml.Append($"<nro>{customer.Address.Number}</nro>");
                xml.Append($"<xBairro>{customer.Address.Neighborhood}</xBairro>");
                xml.Append($"<cMun>{customer.Address.CityCode}</cMun>");
                xml.Append($"<xMun>{customer.Address.City}</xMun>");
                xml.Append($"<UF>{customer.Address.State}</UF>");
                xml.Append($"<CEP>{customer.Address.ZipCode}</CEP>");
                xml.Append($"<cPais>01058</cPais>");
                xml.Append($"<xPais>Brasil</xPais>");
                xml.Append("</ender>");
            }

            xml.Append("</TomS>");
            return xml.ToString();
        }

        private static string GetDetTag(SendNfseInput nfse)
        {
            var xml = new StringBuilder();
            xml.Append("<det>");

            xml.Append("<nItem>1</nItem>");
            xml.Append(GetServTag(nfse.TotalPrice));

            xml.Append("</det>");
            return xml.ToString();
        }

        private static string GetServTag(decimal totalPrice)
        {
            var xml = new StringBuilder();
            xml.Append("<serv>");

            xml.Append("<cServ>1079</cServ>");
            xml.Append("<cLCServ>0104</cLCServ>");
            xml.Append("<xServ>EDICAO</xServ>");
            xml.Append("<uTrib>UN</uTrib>");
            xml.Append("<qTrib>1</qTrib>");
            xml.Append($"<vUnit>{totalPrice.FormatNumber()}</vUnit>");
            xml.Append($"<vServ>{totalPrice.FormatNumber()}</vServ>");
            xml.Append($"<vBCISS>{totalPrice.FormatNumber()}</vBCISS>");
            xml.Append($"<pISS>{GetAliquotIss().FormatNumber()}</pISS>");
            xml.Append($"<vISS>{GetVIssPrice(totalPrice).FormatNumber()}</vISS>");

            xml.Append("</serv>");
            return xml.ToString();
        }

        private static string GetTotalTag(decimal totalPrice)
        {
            var xml = new StringBuilder();
            xml.Append("<total>");

            xml.Append($"<vServ>{totalPrice.FormatNumber()}</vServ>");
            xml.Append($"<vtNF>{totalPrice.FormatNumber()}</vtNF>");
            xml.Append($"<vtLiq>{totalPrice.FormatNumber()}</vtLiq>");
            xml.Append("<ISS>");
            xml.Append($"<vBCISS>{totalPrice.FormatNumber()}</vBCISS>");
            xml.Append($"<vISS>{GetVIssPrice(totalPrice).FormatNumber()}</vISS>");
            xml.Append("</ISS>");

            xml.Append("</total>");
            return xml.ToString();
        }

        private static string GetRefNF(SendNfseInput nfse)
        {
            if (nfse?.Provider == null)
                return string.Empty;

            var ufCode = "43";
            var cnpj = nfse.Provider.Cnpj;
            var model = "90";
            var series = "00S";
            var number = StringExtension.CompleteWithZerosLeft(nfse.Number.ToString(), 9);
            var randomCode = NumberExtension.RandomNumber(9);

            return ufCode + cnpj + model + series + number + randomCode;
        }

        private static decimal GetVIssPrice(decimal totalPrice) => totalPrice * (GetAliquotIss() / 100);

        private static decimal GetAliquotIss() => 4;
    }
}
