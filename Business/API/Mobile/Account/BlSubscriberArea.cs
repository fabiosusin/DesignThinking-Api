using DAO.DBConnection;
using DAO.Mobile.Surf;
using DTO.Mobile.Account.Output;
using System.Collections.Generic;

namespace Business.API.Mobile.Account
{
    public class BlSubscriberArea
    {
        private readonly BlCustomerMsisdn BlCustomerMsisdn;
        private readonly SurfMobilePlanDAO SurfMobilePlanDAO;
        public BlSubscriberArea(XDataDatabaseSettings settings)
        {
            BlCustomerMsisdn = new(settings);
            SurfMobilePlanDAO = new(settings);
        }

        public AppServicesOutput GetServices(string mobileId, string allyId)
        {
            var cellphonesOutput = BlCustomerMsisdn.GetCellphonesByMobileId(mobileId, allyId);
            if (!cellphonesOutput.Success)
                return new(cellphonesOutput.Message);

            var services = new List<AppServiceDetails>();
            foreach (var cellphone in cellphonesOutput.Cellphones)
            {
                var plan = SurfMobilePlanDAO.FindById(cellphone.SurfMobilePlanId);
                services.Add(new(plan, cellphone));
            }

            return new(services);
        }
    }
}
