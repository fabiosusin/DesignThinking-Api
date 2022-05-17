namespace DTO.External.Visao.Input
{
    public class FiltersCameraExternalInput
    {
        public FiltersCameraExternalInput(string cameraId, string allyId)
        {
            CameraId = cameraId;
            AllyId = allyId;
        }

        public bool OthersWithFreeAccess { get; set; }
        public string CameraId { get; set; }
        public string AllyId { get; set; }
    }
}
