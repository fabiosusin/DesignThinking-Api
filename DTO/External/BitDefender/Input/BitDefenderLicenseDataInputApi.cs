using System;

namespace DTO.External.BitDefender.Input
{
    public class BitDefenderLicenseDataInputApi
    {
        public string BitDefenderCategoryId { get; set; }
        public string AllyId { get; set; }
        public string OrderId { get; set; }
        public string Key { get; set; }
        public bool Used { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
