using DTO.General.Base.Api.Output;
using DTO.General.Home;

namespace DTO.Intra.Home.Output
{
    public class IntraHomeDataOutput : BaseApiOutput
    {
        public IntraHomeDataOutput(decimal equipmentQuantity, decimal employeeQuantity, decimal equipmentsLoandedQuantity) : base(true)
        {
            Equipment = new(equipmentQuantity);
            Employee = new(employeeQuantity);
            EquipmentsLoaned = new(equipmentsLoandedQuantity);
        }

        public HomeDataItemInfo Equipment { get; set; }
        public HomeDataItemInfo Employee { get; set; }
        public HomeDataItemInfo EquipmentsLoaned { get; set; }
    }
}
