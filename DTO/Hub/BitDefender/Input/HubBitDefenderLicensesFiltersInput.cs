using System;

namespace DTO.Hub.BitDefender.Input
{
    public class HubBitDefenderLicensesFiltersInput
    {
        public string Key { get; set; }
        public string AllyId { get; set; }
        public string CategoryId { get; set; }
        public long? OrderCode { get; set; }
        public bool? Used { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
