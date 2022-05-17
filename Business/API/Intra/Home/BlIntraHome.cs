using DAO.DBConnection;
using DAO.Intra.Employee;
using DAO.Intra.EquipamentDAO;
using DTO.Intra.Home.Output;

namespace Business.API.Intra.Home
{
    public class BlIntraHome
    {
        private readonly IntraEmployeeDAO IntraEmployeeDAO;
        private readonly IntraEquipmentDAO IntraEquipmentDAO;
        public BlIntraHome(XDataDatabaseSettings settings)
        {
            IntraEmployeeDAO = new(settings);
            IntraEquipmentDAO = new(settings);
        }

        public IntraHomeDataOutput GetHomeData() => new(IntraEquipmentDAO.EquipmentsCount(), IntraEmployeeDAO.EmployeesCount(), IntraEquipmentDAO.EquipmentsLoanedCount());
    }
}
