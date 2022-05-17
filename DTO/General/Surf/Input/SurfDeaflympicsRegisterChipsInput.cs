using System.Collections.Generic;

namespace DTO.General.Surf.Input
{
    public class SurfDeaflympicsRegisterChipsInput
    {
        public string AllyId { get; set; }
        public string MobilePlanId { get; set; }
        public string CustomerId { get; set; }
        public string DDD { get; set; }
        public List<string> Iccids { get; set; }
    }
}
