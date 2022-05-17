using DTO.General.Base.Api.Output;
using DTO.Intra.Equipament.Database;
using System.Collections.Generic;

namespace DTO.Intra.Equipament.Output
{
    public class IntraEquipmentListOutput : BaseApiOutput
    {
        public IntraEquipmentListOutput(string msg) : base(msg) { }
        public IntraEquipmentListOutput(IEnumerable<IntraEquipment> equipments) : base(true) => Equipments = equipments;
        public IEnumerable<IntraEquipment> Equipments { get; set; }
    }
}
