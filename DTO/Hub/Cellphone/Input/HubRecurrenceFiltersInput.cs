using DTO.Hub.Cellphone.Database;
using DTO.Hub.Cellphone.Enum;
using System;

namespace DTO.Hub.Cellphone.Input
{
    public class HubRecurrenceFiltersInput
    {
        public HubRecurrenceFiltersInput() { }
        public HubRecurrenceFiltersInput(SurfCellphoneData cellphoneData) => CellphoneData = cellphoneData;
        public string AllyId { get; set; }
        public SurfCellphoneData CellphoneData { get; set; }
        public string CustomerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public HubCellphoneManagementTypeEnum Type { get; set; }
        public HubCellphoneManagementStatusEnum Status { get; set; }
    }
}
