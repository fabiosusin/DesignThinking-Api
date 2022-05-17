using DTO.General.Base.Database;

namespace DTO.Hub.AccountPlan.Database
{
    public class HubAccountPlan : BaseData
    {
        public HubAccountPlan(string sigeId, string name, string hierarchy, bool outcome)
        {
            SigeId = sigeId;
            Name = name;
            Hierarchy = hierarchy;  
            Expense  = outcome;
        }
        public string Name { get; set; }
        public string Hierarchy{ get; set; }
        public bool Expense { get; set; }
        public string SigeId { get; set; }
    }

}
