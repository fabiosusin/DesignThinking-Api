using DTO.General.Base.Api.Output;

namespace DTO.Hub.NFSe.Output
{
    public class HubNfseXmlFileOutput : BaseApiOutput
    {
        public HubNfseXmlFileOutput() : base(true) { }
        public HubNfseXmlFileOutput(string message) : base(message) { }
        public HubNfseXmlFileOutput(string name, string file) : base(true)
        {
            Name = name;
            File = file;
        }

        public string Name { get; set; }
        public string File { get; set; }
    }
}
