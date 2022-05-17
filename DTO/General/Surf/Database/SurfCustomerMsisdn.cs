using DTO.General.Base.Database;
using DTO.General.Surf.Input;

namespace DTO.General.Surf.Database
{
    public class SurfCustomerMsisdn : BaseData
    {
        public SurfCustomerMsisdn(SurfCustomerMsisdnInput input)
        {
            if (input == null)
                return;

            AllyId = input.AllyId;
            Number = input.Number;
            HubCustomerId = input.HubCustomerId;
            NumberCountryPrefix = input.CountryPrefix;
            CellphoneManagementId = input.CellphoneManagementId;
            FromDeaflympics = input.FromDeaflympics;
        }

        public string AppCustomerId { get; set; }
        public string HubCustomerId { get; set; }
        public string AllyId { get; set; }
        public string Number { get; set; }
        public string NumberCountryPrefix { get; set; }
        public string CellphoneManagementId { get; set; }
        public bool FromDeaflympics { get; set; }
    }
}
