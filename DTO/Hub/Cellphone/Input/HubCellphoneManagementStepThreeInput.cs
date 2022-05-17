namespace DTO.Hub.Cellphone.Input
{
    public class HubCellphoneManagementStepThreeInput : HubBaseCellphoneManagementStepsInput
    {
        public HubCellphoneManagementStepThreeInput(HubCellphoneManagementStepOneInput input, string managementId) : base(managementId) => CustomerId = input.CustomerId;

        public string CustomerId { get; set; }
    }
}
