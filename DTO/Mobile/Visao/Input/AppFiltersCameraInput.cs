using System.Collections.Generic;

namespace DTO.Mobile.Visao.Input
{
    public class AppFiltersCameraInput
    {
        public AppFiltersCameraInput() { }

        public AppFiltersCameraInput(string allyId, bool othersWithFreeAccess)
        {
            AllyId = allyId;
            OthersWithFreeAccess = othersWithFreeAccess;
        }

        public AppFiltersCameraInput(string id, string allyId, bool othersWithFreeAccess)
        {
            CameraId = id;
            AllyId = allyId;
            OthersWithFreeAccess = othersWithFreeAccess;
        }

        public AppFiltersCameraInput(string state, string allyId, bool othersWithFreeAccess, List<string> cities)
        {
            State = state;
            Cities = cities;
            AllyId = allyId;
            OthersWithFreeAccess = othersWithFreeAccess;
        }

        public bool OthersWithFreeAccess { get; set; }
        public string State { get; set; }
        public string CameraName { get; set; }
        public string CameraLink { get; set; }
        public string AllyId { get; set; }
        public string CameraId { get; set; }
        public List<string> Cities { get; set; }
    }
}
