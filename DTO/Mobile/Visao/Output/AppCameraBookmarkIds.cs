using DTO.General.Base.Api.Output;

namespace DTO.Mobile.Visao.Output
{
    public class AppCameraBookmarkIds : BaseApiOutput
    {
        public AppCameraBookmarkIds(string msg) : base(msg) { }
        public AppCameraBookmarkIds(string camId, string userId) : base(true)
        {
            CameraId = camId;
            UserId = userId;
        }
        public string CameraId { get; set; }
        public string UserId { get; set; }
    }
}
