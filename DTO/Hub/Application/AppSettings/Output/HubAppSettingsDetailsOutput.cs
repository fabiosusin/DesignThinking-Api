using DTO.General.Base.Api.Output;

namespace DTO.Hub.Application.AppSettings.Output
{
    public class HubAppSettingsDetailsOutput : BaseApiOutput
    {
        public HubAppSettingsDetailsOutput(string msg) : base(false, msg) { }
        public HubAppSettingsDetailsOutput(Database.AppSettings input) : base(input != null)
        {
            if (input == null)
                return;

            Settings = input;
        }
        public Database.AppSettings Settings { get; set; }
    }
}
