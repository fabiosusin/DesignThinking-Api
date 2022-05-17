using DAO.DBConnection;
using DAO.Hub.AllyDAO;
using DAO.Hub.CustomerDAO;
using DAO.Hub.Order;
using DTO.Hub.Home.Output;

namespace Business.API.Hub.Home
{
    public class BlHubHome
    {
        protected HubOrderDAO HubOrderDAO;
        protected HubAllyDAO HubAllyDAO;
        protected HubCustomerDAO HubCustomerDAO;

        public BlHubHome(XDataDatabaseSettings settings)
        {
            HubCustomerDAO = new(settings);
            HubOrderDAO = new(settings);
            HubAllyDAO = new(settings);
        }

        public HubHomeDataOutput GetHomeData(string allyId)
        {
            if (string.IsNullOrEmpty(allyId))
                return new("Requisição mal formada!");

            var isMasterAlly = HubAllyDAO.FindById(allyId)?.IsMasterAlly ?? false;
            return new HubHomeDataOutput(isMasterAlly ? HubAllyDAO.TotalAlly() : 1, HubOrderDAO.TotalOrder(isMasterAlly ? "" : allyId), HubCustomerDAO.TotalCustomer(isMasterAlly ? "" : allyId));
        }
    }
}
