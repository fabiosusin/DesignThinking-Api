using DTO.General.Surf.Database;
using DTO.Hub.Cellphone.Database;
using DTO.Hub.Cellphone.Enum;
using System;

namespace DTO.Integration.Surf.Recurrence.Input
{
    public class SurfRecurrenceInput
    {
        public SurfRecurrenceInput(SurfDeaflympicsManagement management)
        {
            if (management == null)
                return;

            var msisdn = "";
            if (management.CellphoneData != null)
                msisdn = management.CellphoneData.CountryPrefix + management.CellphoneData.DDD + management.CellphoneData.Number;

            Msisdn = msisdn;
            Iccid = management.Iccid;
            PlanId = management.SurfPlanId;
            SurfCost = NumberToCents(management.Price.SurfPlanPrice);
            CustomerPrice = NumberToCents(management.Price.OrderPrice);
            CuttingCycle = DateTime.Now.ToString("yyyy-MM-dd");
            PaymentDatetime = management.CreationDate.ToString("yyyy-MM-dd HH:mm:ss");
            ExpirationDate = management.CreationDate.AddDays(5).ToString("yyyy-MM-dd");
            PaymentSuccess = false;
            ReferralMonth = management.CreationDate.ToString("yyyy-MM").Replace("-", "/");
        }

        public SurfRecurrenceInput(HubCellphoneManagement management, bool paymentSuccess)
        {
            if (management == null)
                return;

            var msisdn = "";
            if (management.CellphoneData != null)
                msisdn = management.CellphoneData.CountryPrefix + management.CellphoneData.DDD + management.CellphoneData.Number;

            Msisdn = management.Mode == HubCellphoneManagementTypeEnum.Portability ? management.Portability?.Number : msisdn;
            Iccid = management.ChipSerial;
            PlanId = management.SurfPlanId;
            SurfCost = NumberToCents(management.Price.SurfPlanPrice);
            CustomerPrice = NumberToCents(management.Price.OrderPrice);
            CuttingCycle = DateTime.Now.ToString("yyyy-MM-dd");
            PaymentDatetime = management.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");
            ExpirationDate = management.LastUpdate.AddDays(5).ToString("yyyy-MM-dd");
            PaymentSuccess = paymentSuccess;
            ReferralMonth = management.CreationDate.ToString("yyyy-MM").Replace("-", "/");
        }

        public string Msisdn { get; set; }
        public string Iccid { get; set; }
        public string PlanId { get; set; }
        public int SurfCost { get; set; }
        public int CustomerPrice { get; set; }
        public string ReferralMonth { get; set; }
        public string CuttingCycle { get; set; }
        public string PaymentDatetime { get; set; }
        public string ExpirationDate { get; set; }
        public bool PaymentSuccess { get; set; }

        public static int NumberToCents(decimal number) => (int)(number * 100);
    }

}
