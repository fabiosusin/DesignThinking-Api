using System.Collections.Generic;

namespace DTO.Integration.Surf.Operator.Output
{
    public class SurfOperatorOutput
    {
        public string Message { get; set; }
        public List<SurfOperatorPayloadOutput> Payload { get; set; }
    }

    public class SurfOperatorPayloadOutput
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
