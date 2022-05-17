namespace DTO.External.Surf
{
    public class SurfDeaflympicsRegisterOutput
    {
        public SurfDeaflympicsRegisterOutput(string iccid, string msisdn)
        {
            Iccid = iccid;
            Msisdn = msisdn;
        }

        public string Iccid { get; set; }
        public string Msisdn { get; set; }
    }
}
