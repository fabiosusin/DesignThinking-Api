using DTO.General.Base.Database;
using DTO.General.Image.Input;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DTO.Intra.Equipament.Database
{
    public class IntraEquipment : BaseData
    {
        public IntraEquipment() { }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string UserId { get; set; }
        public string DamageNote { get; set; }
        public bool Loaned { get; set; }
        public List<ImageFormat> Images { get; set; }

        [BsonIgnore]
        public List<string> ImagesBase64 { get; set; }
    }
}
