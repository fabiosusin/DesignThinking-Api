using DTO.General.Base.Api.Output;
using DTO.Intra.Equipament.Database;
using DTO.Intra.LoanHistory.Database;
using System.Collections.Generic;

namespace DTO.Intra.EquipamentHistory.Output
{
    public class IntraEquipamentHistoryListOutput : BaseApiOutput
    {
        public IntraEquipamentHistoryListOutput(string msg) : base(msg) { }
        public IntraEquipamentHistoryListOutput(IEnumerable<IntraEquipmentHistory> equipments) : base(true) => EquipmentsHistory = equipments;
        public IEnumerable<IntraEquipmentHistory> EquipmentsHistory { get; set; }
    }
}
