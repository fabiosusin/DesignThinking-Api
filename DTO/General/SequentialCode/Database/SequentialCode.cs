using DTO.General.Base.Database;
using DTO.General.Log.Enum;

namespace DTO.General.SequentialCode.Database
{
    public class SequentialCode : BaseData
    {
        public SequentialCode(SequentialCodeTypeEnum type)
        {
            Type = type;
        }

        public SequentialCode(string dataId, SequentialCodeTypeEnum type)
        {
            DataId = dataId;
            Type = type;
        }

        public string DataId { get; set; }
        public long Code { get; set; }
        public SequentialCodeTypeEnum Type { get; set; }
    }
}
