using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Mobile.Visao.Output
{
    public class AppCameraDetailsOutput : BaseApiOutput
    {
        public AppCameraDetailsOutput(string message) : base(message) { }

        public AppCameraDetailsOutput(AppCameraOutput cam, bool marked, IEnumerable<AppCameraListOutput> relatedCameras) : base(true)
        {
            Bookmark = marked;
            Camera = cam;
            RelatedCameras = relatedCameras;
        }

        public bool Bookmark { get; set; }
        public AppCameraOutput Camera { get; set; }
        public IEnumerable<AppCameraListOutput> RelatedCameras { get; set; }
    }
}
