namespace DTO.Mobile.Visao.Input
{
    public class AppCameraBookmarkInput
    {
        public AppCameraBookmarkInput() { }
        public AppCameraBookmarkInput(string id, string mobileId, string allyId)
        {
            CameraId = id;
            UserMobileId = mobileId;
            AllyId = allyId;
        }

        public string CameraId { get; set; }
        public string AllyId { get; set; }
        public string UserMobileId { get; set; }
    }
}
