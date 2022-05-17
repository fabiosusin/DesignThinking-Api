using DTO.General.Base.Database;

namespace DTO.Mobile.Account.Database
{
    public class AppCustomerAccount : BaseData
    {
        public string Name { get; set; }
        public string AllyId { get; set; }
        public string Cellphone { get; set; }
        public string CellphoneCountryPrefix { get; set; }
    }
}
