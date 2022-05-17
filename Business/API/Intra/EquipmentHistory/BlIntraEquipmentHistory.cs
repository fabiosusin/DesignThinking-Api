using DAO.DBConnection;
using DAO.Intra.LoanHistory;
using DTO.Intra.EquipamentHistory.Input;
using DTO.Intra.EquipamentHistory.Output;
using System.Linq;

namespace Business.API.Intra.EquipmentHistory
{
    public class BlIntraEquipmentHistory
    {
        private readonly IntraEquipmentHistoryDAO IntraEquipmentHistoryDAO;

        public BlIntraEquipmentHistory(XDataDatabaseSettings settings)
        {
            IntraEquipmentHistoryDAO = new(settings);
        }

        public IntraEquipamentHistoryListOutput List(IntraEquipmentHistoryListInput input)
        {
            var result = IntraEquipmentHistoryDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum histórico encontrado!");

            return new(result);
        }
    }
}
