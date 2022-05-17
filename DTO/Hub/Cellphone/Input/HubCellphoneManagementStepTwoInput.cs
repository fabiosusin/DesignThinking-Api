namespace DTO.Hub.Cellphone.Input
{
    public class HubCellphoneManagementStepTwoInput : HubBaseCellphoneManagementStepsInput
    {
        public HubCellphoneManagementStepTwoInput(string managementId, string code) : base(managementId) => SurfCustomerCode = code;

        public string SurfCustomerCode { get; set; }
    }
}
