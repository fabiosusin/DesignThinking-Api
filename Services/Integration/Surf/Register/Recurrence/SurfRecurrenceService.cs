using DTO.Integration.Surf.Recurrence.Input;
using DTO.Integration.Surf.Recurrence.Output;
using System.Threading.Tasks;

namespace Services.Integration.Surf.Register.Recurrence
{
    public class SurfRecurrenceService
    {
        internal SurfApiService SurfApiService;
        public SurfRecurrenceService() => SurfApiService = new();

        public async Task<SurfRecurrenceOutput> AddRecurrence(SurfRecurrenceInput input) => await SurfApiService.AddRecurrence(input).ConfigureAwait(false);
    }
}
