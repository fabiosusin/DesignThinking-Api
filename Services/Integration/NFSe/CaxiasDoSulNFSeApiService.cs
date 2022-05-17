using DTO.General.Api.Enum;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Useful.Service;

namespace Services.Integration.NFSe
{
    internal class CaxiasDoSulNFSeApiService
    {
        private readonly ApiDispatcher _apiDispatcher;
        private static readonly string BaseUrl = EnvironmentService.Get() == EnvironmentService.Dev ? "https://nfsehomol.caxias.rs.gov.br/services/nfse/ws/Servicos" : "https://nfse.caxias.rs.gov.br/services/nfse/ws/Servicos";

        public CaxiasDoSulNFSeApiService() => _apiDispatcher = new ApiDispatcher();

        public async Task<T> SendNfseRequest<T>(string xml) => GetResponse<T>(await SendRequest(xml));

        private static T GetResponse<T>(string xml)
        {
            var xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xml);

            using var reader = new StringReader(xmlResponse.InnerText);
            return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
        }

        private static Tuple<HttpRequestHeader, string>[] DefaultHeaders() => new Tuple<HttpRequestHeader, string>[]
           {
                new Tuple<HttpRequestHeader, string>(HttpRequestHeader.ContentType, "text/xml; charset=utf-8")
           };

        private async Task<string> SendRequest(string xml)
        {
            try
            {
                var body = new StringBuilder();

                body.Append(GetTopBody());
                body.Append(xml);
                body.Append(GetFooterBody());

                return await _apiDispatcher.DispatchWithResponseUnDeserializeAsync(BaseUrl, RequestMethodEnum.POST, body.ToString(), DefaultHeaders());
            }
            catch 
            {
                throw;
            }
        }

        private static string GetTopBody() => @"<soap:Envelope xmlns=""http://ws.pc.gif.com.br/"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><soap:Body>";

        private static string GetFooterBody() => @"</soap:Body></soap:Envelope>";
    }
}
