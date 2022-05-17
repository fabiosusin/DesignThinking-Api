using DTO.General.Base.Api.Output;

namespace DTO.Hub.Cellphone.Output
{
    public class HubCellphoneManagementOutput : BaseApiOutput
    {
        public HubCellphoneManagementOutput(bool success) : base(success) { }
        public HubCellphoneManagementOutput(string msg) : base(msg) { }
        public HubCellphoneManagementOutput(bool success, string id) : base(success) => CellphoneManagementId = id;
        public string CellphoneManagementId { get; set; }
    }
}
