namespace DTO.Hub.Cellphone.Input
{
    public class HubBaseCellphoneManagementStepsInput
    {
        public HubBaseCellphoneManagementStepsInput(string managementId) => CellphoneManagementId = managementId;

        public string CellphoneManagementId { get; set; }
    }
}
