using Business.API.Hub.NFSe;
using DAO.DBConnection;
using DAO.Hub.Order;
using DTO.General.Files.Output;
using DTO.Hub.NFSe.Output;
using DTO.Hub.Order.Output;
using System;
using System.IO;
using System.Threading.Tasks;
using Useful.Extensions;
using Useful.Service;

namespace Business.API.General.Files
{
    public class BlOrderNfseXml : BlFileAbstract
    {
        private readonly BlNfse BlNfse;
        private readonly HubOrderDAO HubOrderDAO;
        public BlOrderNfseXml(XDataDatabaseSettings settings)
        {
            BlNfse = new(settings);
            HubOrderDAO = new(settings);
        }

        public override async Task<GenerateDocOutput> GenerateDoc(string id)
        {
            var order = HubOrderDAO.FindById(id);
            if (order == null)
                return null;

            var nfseDetails = await BlNfse.GetNfseDetails(id);
            if (!nfseDetails.Success)
                return null;

            var result = new HubNfseXmlFileOutput
            {
                File = $"{EnvironmentService.DocumentBasePath}\\{Guid.NewGuid()}.xml"
            };

            File.WriteAllText(result.File, StringExtension.ObjectToXML(nfseDetails.NfseOutput));
            return new($"nfse_pedido_{order.Code}", result.File);
        }
    }
}
