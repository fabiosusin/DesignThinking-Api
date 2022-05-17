using Business.API.Mobile.Surf;
using DAO.DBConnection;
using DAO.General.Surf;
using DAO.Hub.Application.Database;
using DAO.Mobile.Account;
using DTO.General.Image.Input;
using DTO.Mobile.Account.Database;
using DTO.Mobile.Account.Output;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API.Mobile.Account
{
    public class BlHome
    {
        private readonly BlPlanDetails BlPlanDetails;
        private readonly AppSponsorDAO AppSponsorDAO;
        private readonly BlCustomerMsisdn BlCustomerMsisdn;
        private readonly MobileAccountDAO MobileAccountDAO;
        private readonly SurfCustomerMsisdnDAO SurfCustomerMsisdnDAO;
        public BlHome(XDataDatabaseSettings settings)
        {
            BlPlanDetails = new(settings);
            AppSponsorDAO = new(settings);
            MobileAccountDAO = new(settings);
            BlCustomerMsisdn = new(settings);
            SurfCustomerMsisdnDAO = new(settings);
        }

        public async Task<AppCelularHomeInfoOutput> GetRegisteredCellPhones(string mobileId, string allyId)
        {
            var cellphonesOutput = BlCustomerMsisdn.GetCustomerMsisdnList(mobileId, allyId);
            if (!cellphonesOutput.Success)
                return new(cellphonesOutput.Message);

            var customerCellphones = new List<MobileCellphoneData>();
            foreach (var item in cellphonesOutput.Cellphones)
            {
                var data = await BlPlanDetails.GetAccountPlanInfo(item.NumberCountryPrefix + item.Number).ConfigureAwait(false);
                if (data == null)
                    continue;

                _ = long.TryParse(item.Number, out var number);
                customerCellphones.Add(new MobileCellphoneData
                {
                    Call = data.Call,
                    Internet = data.Internet,
                    Sms = data.Sms,
                    Msisdn = number,
                    MsisdnFormatted = number > 0 ? string.Format(number.ToString().Length > 10 ? "{0:(##) #####-####}" : "{0:(##) ####-####}", number) : item.Number,
                    PlanName = data.Name
                });
            }

            return new(customerCellphones);
        }

        public AppMobileHomeSponsorsOutput GetHomeSponsorsOutput(string allyId, ListResolutionsSize imgSize)
        {
            if (string.IsNullOrEmpty(allyId))
                return new("Id de Aliado não informado!");

            var sponsors = AppSponsorDAO.Find(x => x.AllyId == allyId);
            return !(sponsors?.Any() ?? false) ? new("Patrocinadores não encontrados") : new(sponsors.Select(x => new MobileHomeSponsorsData(x, imgSize)));
        }

    }
}
