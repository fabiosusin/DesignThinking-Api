using DTO.External.Visao.Database;
using DTO.General.Base.Api.Output;

namespace DTO.External.Visao.Output
{
    public class CameraExternalOutput : BaseApiOutput
    {
        public CameraExternalOutput(string msg) : base(msg) { }
        public CameraExternalOutput(VisaoCamera camera) : base(true)
        {
            Camera = camera;
        }

        public VisaoCamera Camera { get; set; }
    }
}
