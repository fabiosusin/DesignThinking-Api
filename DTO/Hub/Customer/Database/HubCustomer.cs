using DTO.General.Address.Input;
using DTO.General.Base.Database;
using DTO.Hub.Ally.Enum;
using DTO.Hub.Cellphone.Database;
using DTO.Hub.Cellphone.Enum;
using DTO.Hub.Customer.Enum;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DTO.Hub.Customer.Database
{
    public class HubCustomer : BaseData
    {
        public string Email { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
        public string Notes { get; set; }
        public string Name { get; set; }
        public string AllyId { get; set; }
        public string Representative { get; set; }
        public string AsaasId { get; set; }
        public HubIcmsTypeEnum IcmsType { get; set; }
        public DateTime BirhDay { get; set; }
        public HubSexTypeEnum Sex { get; set; }
        public SurfCellphoneData CellphoneData { get; set; }
        public DocumentData Document { get; set; }
        public RgData Rg { get; set; }
        public Address Address { get; set; }

        [BsonIgnore]
        public string DocumentBase64 { get; set; }
    }

    public class DocumentData
    {
        public string Data { get; set; }
        public string ImageLink { get; set; }
        public HubDocumentTypeEnum Type { get; set; }
    }

    public class RgData
    {
        public string Number { get; set; }
        public string Issuer { get; set; }
        public DateTime? ExpeditionDate { get; set; }
    }

    public class PhoneData
    {
        public long Residential { get; set; }
        public long Commercial { get; set; }
        public long Cellphone { get; set; }
        public HubCellphoneTypeEnum CellphoneType { get; set; }
    }
}
