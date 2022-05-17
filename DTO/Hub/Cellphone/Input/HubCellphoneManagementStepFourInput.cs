using DTO.Hub.Cellphone.Database;
using DTO.Hub.Cellphone.Enum;

namespace DTO.Hub.Cellphone.Input
{
    public class HubCellphoneManagementStepFourInput : HubBaseCellphoneManagementStepsInput
    {
        public HubCellphoneManagementStepFourInput(HubCellphoneManagementStepOneInput input, string managementId) : base(managementId)
        {
            Mode = input.Mode;
            Portability = input.Portability;
        }

        public CellphoneManagementPortability Portability { get; set; }
        public HubCellphoneManagementTypeEnum Mode { get; set; }
        
    }
}
