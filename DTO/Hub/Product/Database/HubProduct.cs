using DTO.External.BitDefender.Database;
using DTO.General.Base.Database;
using DTO.Hub.Product.Output;
using DTO.Integration.Sige.Product.Output;
using MongoDB.Bson.Serialization.Attributes;

namespace DTO.Hub.Product.Database
{
    public class HubProduct : BaseData
    {
        public HubProduct() { }
        public HubProduct(HubProduct input)
        {
            if (input == null)
                return;

            Id = input.Id;
            Code = input.Code;
            Name = input.Name;
            SigeId = input.SigeId;
            CategoryId = input.CategoryId;
            SurfMobilePlanId = input.SurfMobilePlanId;
            BitDefenderCategoryId = input.BitDefenderCategoryId;
            Price = input.Price;
        }

        public HubProduct(SigeProductInput input)
        {
            Code = input.Codigo;
            Name = input.Nome;
            SigeId = input.Id;
        }

        public HubProduct(BitDefenderCategory bitDefenderCategory, string categoryId)
        {
            if (bitDefenderCategory == null)
                return;

            Name = bitDefenderCategory.Name;
            CategoryId = categoryId;
            BitDefenderCategoryId = bitDefenderCategory.Id;
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string SigeId { get; set; }
        public string AllyId { get; set; }
        public string CategoryId { get; set; }
        public string SurfMobilePlanId { get; set; }
        public string BitDefenderCategoryId { get; set; }

        [BsonIgnore]
        public HubProductPriceTablePrice Price { get; set; }
    }
}
