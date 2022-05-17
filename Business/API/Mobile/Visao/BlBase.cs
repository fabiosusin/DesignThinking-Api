using DAO.DBConnection;
using DAO.External.Visao;
using DAO.Mobile.Account;
using DAO.Mobile.Visao;

namespace Business.API.Mobile.Visao
{
    public class BlBase
    {
        protected VisaoCameraDAO VisaoCameraDAO; 
        protected MobileAccountDAO MobileAccountDAO; 
        protected VisaoFavoriteCamerasDAO VisaoFavoriteCamerasDAO;

        public BlBase(XDataDatabaseSettings settings) 
        {
            VisaoCameraDAO = new(settings);
            MobileAccountDAO = new(settings);
            VisaoFavoriteCamerasDAO = new(settings);
        }
    }
}
