using DAO.DBConnection;
using DAO.General.Surf;
using DAO.Hub.Cellphone;
using DTO.General.Surf.Database;
using DTO.Mobile.Account.Output;
using System.Linq;

namespace Business.API.Mobile.Account
{
    public class BlCustomerMsisdn
    {
        private readonly BlAppAccount BlAppAccount;
        private readonly SurfCustomerMsisdnDAO SurfCustomerMsisdnDAO;
        private readonly HubCellphoneManagementDAO HubCellphoneManagementDAO;
        public BlCustomerMsisdn(XDataDatabaseSettings settings)
        {
            BlAppAccount = new(settings);
            SurfCustomerMsisdnDAO = new(settings);
            HubCellphoneManagementDAO = new(settings);
        }

        public SurfCustomerMsisdn GetMsisdn(string mobileId)
        {
            if (string.IsNullOrEmpty(mobileId))
                return null;

            if (mobileId.Length == 13)
                mobileId = mobileId.Remove(0, 2);

            return SurfCustomerMsisdnDAO.FindOne(x => x.Number == mobileId);
        }

        public AppCustomerCellphonesOutput GetCellphonesByMobileId(string mobileId, string allyId)
        {
            var msisdns = GetCustomerMsisdnList(mobileId, allyId);
            if (!msisdns.Success)
                return new(msisdns.Message);

            var ids = msisdns.Cellphones.Select(x => x.CellphoneManagementId);
            return new(HubCellphoneManagementDAO.Find(x => ids.Contains(x.Id)));
        }

        public AppCustomerMsisdOutput GetCustomerMsisdnList(string mobileId, string allyId)
        {
            if (string.IsNullOrEmpty(allyId))
                return new("Id de aliado não informado!");

            if (string.IsNullOrEmpty(mobileId))
                return new("Email de usuário não informado!");

            var accountId = BlAppAccount.FindAccountByMobileId(new(mobileId, allyId))?.Id;
            if (string.IsNullOrEmpty(accountId))
                return new("Usuário não encontrado a partir destes dados!");

            return new(SurfCustomerMsisdnDAO.Find(x => x.AllyId == allyId && x.AppCustomerId == accountId));
        }
    }
}
