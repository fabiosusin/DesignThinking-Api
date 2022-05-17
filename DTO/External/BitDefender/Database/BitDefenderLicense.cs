using DTO.External.BitDefender.Input;
using DTO.General.Base.Database;
using System;

namespace DTO.External.BitDefender.Database
{
    public class BitDefenderLicense : BaseData
    {
        public BitDefenderLicense() { }
        public BitDefenderLicense(BitDefenderLicenseDataInputApi input, string categoryId)
        {
            if (input == null)
                return;

            BitDefenderCategoryId = categoryId;
            Key = input.Key;
            AllyId = input.AllyId;
            OrderId = input.OrderId;
            Used = input.Used;
            RegisterDate = input.RegisterDate;
            LastUpdate = input.LastUpdate;
        }

        public string BitDefenderCategoryId { get; set; }
        public string Key { get; set; }
        public string AllyId { get; set; }
        public string OrderId { get; set; }
        public bool Used { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
