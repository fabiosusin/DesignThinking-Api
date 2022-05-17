using DTO.General.Base.Database;
using DTO.Hub.Application.AppSettings.Database;
using DTO.Mobile.Surf.Enum;
using DTO.Surf.Enum;

namespace DTO.Hub.Application.AppSettings.Input
{
    public class HubAppSettingsInput
    {
        public string Id { get; set; }
        public string AllyId { get; set; }
        public string MainColor { get; set; }
        public AppSegmentsConfigs TV { get; set; }
        public AppSegmentsConfigs Visao360 { get; set; }
        public AppSegmentsConfigs Celular { get; set; }
        public SupportConfigs SupportConfigs { get; set; }
        public AppPartnerTypeEnum Type { get; set; }
        public AppToolsEnum Tools { get; set; }
    }
}
