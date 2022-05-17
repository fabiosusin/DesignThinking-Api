using DTO.General.Base.Database;

namespace DTO.External.BitDefender.Database
{
    public class BitDefenderCategory : BaseData
    {
        public BitDefenderCategory() { }
        public BitDefenderCategory(string name) => Name = name;

        public string Name { get; set; }
    }
}
