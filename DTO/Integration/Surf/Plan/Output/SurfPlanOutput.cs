using System.Collections.Generic;

namespace DTO.Integration.Surf.Plan.Output
{
    public class SurfPlanOutput
    {
        public string Message { get; set; }
        public List<SurfPayloadPlanOutput> Payload { get; set; }
    }

    public class SurfPayloadPlanOutput
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string DurationTime { get; set; }
        public string Price { get; set; }
        public string SurfCost { get; set; }
        public List<SurfPlanAdvantageOutput> Advantages { get; set; }
    }

    public class SurfPlanAdvantageOutput
    {
        public string Description { get; set; }
    }

    public static class SurfPayloadTypes
    {
        public const string Single = "single";
        public const string Addon = "addon";
        public const string Recharge = "recharge";
        public const string Multiple = "multiple";
    }
}
