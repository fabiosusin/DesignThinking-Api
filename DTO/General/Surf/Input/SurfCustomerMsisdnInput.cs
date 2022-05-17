using DTO.General.Surf.Database;
using DTO.Hub.Cellphone.Database;

namespace DTO.General.Surf.Input
{
    public class SurfCustomerMsisdnInput
    {
        public SurfCustomerMsisdnInput(string number, string countryPrefix, string allyId)
        {
            AllyId = allyId;
            CountryPrefix = countryPrefix;
            Number = number;
        }

        public SurfCustomerMsisdnInput(SurfDeaflympicsManagement management)
        {
            if (management == null)
                return;

            AllyId = management.AllyId;
            HubCustomerId = management.CustomerId;
            CellphoneManagementId = management.Id;
            FromDeaflympics = true;

            if (management.CellphoneData == null)
                return;

            CountryPrefix = management.CellphoneData.CountryPrefix;
            Number = management.CellphoneData.DDD + management.CellphoneData.Number;
        }

        public SurfCustomerMsisdnInput(HubCellphoneManagement management)
        {
            if (management == null)
                return;

            AllyId = management.AllyId;
            HubCustomerId = management.CustomerId;
            CellphoneManagementId = management.Id;

            if (management.CellphoneData == null)
                return;

            CountryPrefix = management.CellphoneData.CountryPrefix;
            Number = management.CellphoneData.DDD + management.CellphoneData.Number;
        }

        public string HubCustomerId { get; set; }
        public string Number { get; set; }
        public string CountryPrefix { get; set; }
        public string AllyId { get; set; }
        public string CellphoneManagementId { get; set; }
        public bool FromDeaflympics { get; set; }
    }
}
