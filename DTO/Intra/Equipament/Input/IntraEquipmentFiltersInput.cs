using System.Collections.Generic;

namespace DTO.Intra.Equipament.Input
{
    public class IntraEquipmentFiltersInput
    {
        public IntraEquipmentFiltersInput() { }
        public IntraEquipmentFiltersInput(List<string> ids) => Ids = ids;
        public List<string> Ids { get; set; }
        public string Name { get; set; }
        public bool? Loaned { get; set; }
    }
}
