using DTO.General.Base.Api.Output;
using DTO.Hub.Cellphone.Enum;
using System.Collections.Generic;

namespace DTO.Hub.Order.Output
{
    public class HubOrderRegisterCellphoneNumbersOutput : BaseApiOutput
    {
        public HubOrderRegisterCellphoneNumbersOutput(string message) : base(false, message) { }
        public HubOrderRegisterCellphoneNumbersOutput(HubCellphoneManagementStatusEnum status, List<HubProductOrderResultCellphoneData> cellphoneNumbers) : base(true)
        {
            Status = status;
            CellphoneNumbers = cellphoneNumbers;
        }
        public HubOrderRegisterCellphoneNumbersOutput(HubCellphoneManagementStatusEnum status, string message, List<HubProductOrderResultCellphoneData> cellphoneNumbers) : base(false, message)
        {
            Status = status;
            CellphoneNumbers = cellphoneNumbers;
        }

        public HubCellphoneManagementStatusEnum Status { get; set; }
        public List<HubProductOrderResultCellphoneData> CellphoneNumbers { get; set; }
    }
}
