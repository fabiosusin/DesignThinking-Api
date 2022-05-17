using DTO.Integration.Surf.Portability.Input;
using DTO.Integration.Surf.Portability.Output;
using System.Threading.Tasks;

namespace Services.Integration.Surf.Register.Portability
{
    public class SurfPortabilityService
    {
        internal SurfApiService SurfApiService;
        public SurfPortabilityService() => SurfApiService = new();

        public async Task<SurfPortabilityOutput> AddPortability(SurfPortabalityInput input) => await SurfApiService.AddPortability(input).ConfigureAwait(false);

        public async Task<SurfCheckPortabilityStatusOutput> CheckPortabilityStatus(SurfCheckPortabilityStatusInput input) => await SurfApiService.ChekPortabilityStatus(input).ConfigureAwait(false);

        public async Task<SurfResendPortabilitySmsOutput> ResendPortabilitySms(SurfResendPortabilitySmsInput input) => await SurfApiService.ResendPortabilitySms(input).ConfigureAwait(false);
    }
}
