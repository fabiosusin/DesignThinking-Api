using DTO.External.Visao.Database;
using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.External.Visao.Output
{
    public class CameraListExternalOutput : BaseApiOutput
    {
        public CameraListExternalOutput(string msg) : base(msg) { }
        public CameraListExternalOutput(List<VisaoCamera> cameras) : base(true)
        {
            Cameras = cameras;
        }

        public List<VisaoCamera> Cameras { get; set; }
    }
}
