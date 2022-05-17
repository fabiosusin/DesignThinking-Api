using DTO.General.Base.Database;
using DTO.Hub.Order.Enum;
using DTO.Hub.Order.Input;
using DTO.Hub.Product.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTO.Hub.Order.Database
{
    public class HubOrder : BaseData
    {
        public HubOrder() { }
        public HubOrder(HubOrderInput input, OrderPrice price, string customerDocumentLink)
        {
            if (input == null)
                return;

            Customer = new(input.Customer?.Id, customerDocumentLink);
            SellerId = input.SellerId;
            AllyId = input.Ally?.Id;
            AccountPlanId = input.AccountPlanId;
            CreationDate = DateTime.Now;
            Price = price;
            Payments = input.Payments?.Select(x => new HubOrderPaymentData(x.Type, x.Value))?.ToList();
            CompanyId = input.CompanyId;
            DepositId = input.DepositId;
            Status = HubOrderStatusEnum.Created;
        }

        public long Code { get; set; }
        public long SigeCode { get; set; }
        public string SellerId { get; set; }
        public string AllyId { get; set; }
        public string CompanyId { get; set; }
        public string DepositId { get; set; }
        public string AccountPlanId { get; set; }
        public DateTime CreationDate { get; set; }
        public HubOrderStatusEnum Status { get; set; }
        public HubOrderNfse Nfse { get; set; }
        public HubCustomerOrderData Customer { get; set; }
        public OrderPrice Price { get; set; }
        public List<HubOrderPaymentData> Payments { get; set; }
    }

    public class HubOrderNfse
    {
        public HubOrderNfse() { }
        public HubOrderNfse(string lot) => Lot = lot;
        public HubOrderNfse(string lot, string key)
        {
            Lot = lot;
            AccessKey = key;
        }

        public string AccessKey { get; set; }
        public string Lot { get; set; }
    }

    public class HubCustomerOrderData
    {
        public HubCustomerOrderData(string id, string link)
        {
            CustomerId = id;
            DocumentLink = link;
        }

        public string CustomerId { get; set; }
        public string DocumentLink { get; set; }
    }

    public class OrderPrice : HubProductPriceTablePrice
    {
        public decimal Discount { get; set; }
    }

    public class HubOrderPaymentData
    {
        public HubOrderPaymentData() { }
        public HubOrderPaymentData(HubOrderPaymentFormEnum type, decimal value)
        {
            Type = type;
            Value = value;
        }

        public HubOrderPaymentFormEnum Type { get; set; }
        public decimal Value { get; set; }
    }
}
