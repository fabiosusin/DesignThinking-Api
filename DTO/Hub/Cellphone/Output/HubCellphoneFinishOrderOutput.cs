using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Hub.Cellphone.Output
{
    public class HubCellphoneFinishOrderOutput : BaseApiOutput
    {
        public HubCellphoneFinishOrderOutput(bool success) : base(success) { }
        public HubCellphoneFinishOrderOutput(string msg) : base(msg) { }
        public HubCellphoneFinishOrderOutput(string msg, List<HubFinishCellphoneResult> cellphones) : base(msg) => CellphonesWithErrors = cellphones;

        public List<HubFinishCellphoneResult> CellphonesWithErrors { get; set; }
    }

    public class HubFinishCellphoneResult
    {
        public HubFinishCellphoneResult(string productId)
        {
            ProductOrderId = productId;
            Success = true;
        }

        public string ProductOrderId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
