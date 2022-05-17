using DTO.General.Files.Output;
using System.Threading.Tasks;

namespace Business.API.General.Files
{
    public abstract class BlFileAbstract
    {
        public abstract Task<GenerateDocOutput> GenerateDoc(string id);
    }
}