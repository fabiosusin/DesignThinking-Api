using DTO.General.Base.Api.Output;
using DTO.Integration.Sige.Entry.Input;
using System.Threading.Tasks;

namespace Services.Integration.Sige.Entry
{
    public class SigeEntryService
    {
        internal SigeApiService SigeApiService;
        public SigeEntryService() => SigeApiService = new();

        public async Task<BaseApiOutput> CreateEntry(SigeEntryInput input) => await SigeApiService.CreateEntry(input);
    }
}
