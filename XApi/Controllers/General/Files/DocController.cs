using Business.API.General.Files;
using DAO.DBConnection;
using DTO.General.Files.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using Useful.Extensions.FilesExtension;

namespace XApi.Controllers.General.Files
{
    [ApiController]
    public class DocController : FilesBaseController<DocController>
    {
        private readonly XDataDatabaseSettings Settings;
        public DocController(ILogger<DocController> logger, XDataDatabaseSettings settings) : base(logger) => Settings = settings;

        [HttpGet, Route("generate-doc"), AllowAnonymous]
        public async Task<IActionResult> GenerateContractAsync(DocTypeEnum type, string id)
        {
            var doc = await Create(Settings, type).GenerateDoc(id).ConfigureAwait(false);
            var fileInfo = new FileInfo(doc.Path);
            return string.IsNullOrEmpty(fileInfo.Extension) ? BadRequest() : Ok(File(FilesExtension.GetByteFromFile(doc.Path), FilesExtension.GetContentType(fileInfo.Extension), doc.Name + fileInfo.Extension));
        }

        private static BlFileAbstract Create(XDataDatabaseSettings settings, DocTypeEnum type)
        {
            return type switch
            {
                DocTypeEnum.OrderContract => new BlOrderContract(settings),
                DocTypeEnum.OrderNfseXml => new BlOrderNfseXml(settings),
                DocTypeEnum.OrderNfse => new BlOrderNfse(settings),
                DocTypeEnum.LoanContract => new BlLoanContract(settings),
                _ => null
            };
        }

    }
}
