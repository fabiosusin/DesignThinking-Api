using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Mobile.Visao.Output
{
    public class AppCameraListOutput : BaseApiOutput
    {
        public AppCameraListOutput(string msg) : base(msg) { }

        public AppCameraListOutput(string city, string state, int online, List<AppCameraOutput> cameras) : base(true)
        {
            City = city;
            State = state;
            CamerasOnline = online;
            Cameras = cameras;
        }

        public string City { get; set; }
        public string State { get; set; }
        public int CamerasOnline { get; set; }
        public List<AppCameraOutput> Cameras { get; set; }
    }
}
