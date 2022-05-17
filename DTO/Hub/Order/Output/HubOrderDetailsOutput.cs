using DTO.General.Address.Input;
using DTO.General.Base.Api.Output;
using DTO.General.Base.Output;
using DTO.Hub.BitDefender.Output;
using DTO.Hub.Cellphone.Database;
using DTO.Hub.Customer.Database;
using DTO.Hub.Order.Database;
using DTO.Hub.Order.Enum;
using DTO.Hub.Order.Input;
using DTO.Hub.Product.Database;
using System;
using System.Collections.Generic;

namespace DTO.Hub.Order.Output
{
    public class HubOrderDetailsOutput : BaseApiOutput
    {
        public HubOrderDetailsOutput(bool success, HubOrderOutput order, List<HubProductOrderDetailsOutput> products) : base(success)
        {
            Order = order;
            ProductsOrder = products;
        }
        public HubOrderDetailsOutput(string msg) : base(msg) { }

        public HubOrderOutput Order { get; set; }
        public List<HubProductOrderDetailsOutput> ProductsOrder { get; set; }
    }

    public class HubOrderOutput
    {
        public HubOrderOutput(HubOrder order, HubCustomer customer)
        {
            if (order == null)
                return;

            Id = order.Id;
            Code = order.Code;
            AccountPlanId = order.AccountPlanId;
            Ally = new HubOrderAllyInput(order.AllyId, null);
            CompanyId = order.CompanyId;
            Nfse = order.Nfse;
            Customer = new(customer, order.Customer?.DocumentLink);
            SellerId = order.SellerId;
            Status = order.Status;
            CreationDate = order.CreationDate;
        }

        public HubCustomerOrderDetails Customer { get; set; }
        public string Id { get; set; }
        public string SellerId { get; set; }
        public string AccountPlanId { get; set; }
        public string CompanyId { get; set; }
        public long Code { get; set; }
        public DateTime CreationDate { get; set; }
        public HubOrderNfse Nfse { get; set; }
        public HubOrderAllyInput Ally { get; set; }
        public HubOrderStatusEnum Status { get; set; }
        public IEnumerable<HubOrderCreationChargeOutput> Payments { get; set; }
    }

    public class HubCustomerOrderDetails : BaseInfoOutput
    {
        public HubCustomerOrderDetails(HubCustomer customer, string documentLink) : base(customer?.Id, customer?.Name)
        {
            DocumentLink = documentLink;
            if (customer == null)
                return;

            BirhDay = customer.BirhDay;
            Email = customer.Email;
            CellphoneData = customer.CellphoneData;
            Document = customer.Document;
            Address = customer.Address;
        }

        public string Email { get; set; }
        public string DocumentLink { get; set; }
        public DateTime BirhDay { get; set; }
        public SurfCellphoneData CellphoneData { get; set; }
        public DocumentData Document { get; set; }
        public Address Address { get; set; }
    }

    public class HubProductOrderDetailsOutput
    {
        public HubProductOrderDetailsOutput(HubProduct product, string productOrderId)
        {
            if (product == null)
                return;

            ProductOrderId = productOrderId;
            ProductId = product.Id;
            CategoryId = product.CategoryId;
            Name = product.Name;
            Code = product.Code;
            Quantity = 1;
        }
        public string ProductId { get; set; }
        public string ProductOrderId { get; set; }
        public string CategoryId { get; set; }
        public decimal Quantity { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public HubProductOrderResultCellphoneData CellphoneData { get; set; }
        public BitDefenderLicensesOutput BitDefenderData { get; set; }
        public HubProductPriceTablePrice Price { get; set; }
    }
}
