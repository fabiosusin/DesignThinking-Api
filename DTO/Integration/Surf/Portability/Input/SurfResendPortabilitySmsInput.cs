using DTO.Hub.Cellphone.Database;

namespace DTO.Integration.Surf.Portability.Input
{
    public class SurfResendPortabilitySmsInput
    {
        public SurfResendPortabilitySmsInput(string msisdn, string pmsisdn, string document)
        {
            Pmsisdn = pmsisdn;
            Msisdn = msisdn;
            Document = document;
        }

        public string Msisdn { get; set; }
        public string Pmsisdn { get; set; }
        public string Document { get; set; }
    }
}
