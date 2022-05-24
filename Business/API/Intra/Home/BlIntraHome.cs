using DAO.DBConnection;
using DAO.Intra.Employee;
using DTO.Intra.Home.Output;

namespace Business.API.Intra.Home
{
    public class BlIntraHome
    {
        private readonly IntraEmployeeDAO IntraEmployeeDAO;
        public BlIntraHome(XDataDatabaseSettings settings)
        {
            IntraEmployeeDAO = new(settings);
        }

        public IntraHomeDataOutput GetHomeData() => new(IntraEmployeeDAO.EmployeesCount());
    }
}
