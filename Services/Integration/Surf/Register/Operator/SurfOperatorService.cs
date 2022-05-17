using DTO.Integration.Surf.Operator.Output;
using System.Threading.Tasks;

namespace Services.Integration.Surf.Register.Operator
{
    public class SurfOperatorService
    {
        internal SurfApiService SurfApiService;
        public SurfOperatorService() => SurfApiService = new();

        public async Task<SurfOperatorOutput> GetOperators() => await SurfApiService.GetOperators().ConfigureAwait(false);
    }
}
