using DTO.General.Base.Api.Output;
using DTO.Hub.Order.Database;
using DTO.Hub.Order.Enum;
using System;
using System.Collections.Generic;

namespace DTO.Hub.Order.Output
{
    public class HubOrderListOutput : BaseApiOutput
    {
        public HubOrderListOutput(string msg) : base(msg) { }
        public HubOrderListOutput(IEnumerable<HubOrderListData> allys) : base(true) => Orders = allys;
        public IEnumerable<HubOrderListData> Orders { get; set; }
    }

    public class HubOrderListData
    {
        public HubOrderListData(HubOrder order)
        {
            if (order == null)
                return;

            Id = order.Id;
            Code = order.Code;
            Date = order.CreationDate;
            OrderPrice = order.Price?.Price ?? 0;
            Discount = order.Price?.Discount ?? 0;
            Status = order.Status;
            Payments = order.Payments;
            Share = new(order.Price);
        }
        public string Id { get; set; }
        public long Code { get; set; }
        public string CustomerName { get; set; }
        public string SellerName { get; set; }
        public string AccountPlanName { get; set; }
        public decimal Discount { get; set; }
        public decimal OrderPrice { get; set; }
        public DateTime Date { get; set; }
        public HubOrderStatusEnum Status { get; set; }
        public HubOrderListShare Share { get; set; }
        public List<HubOrderPaymentData> Payments { get; set; }
    }

    public class HubOrderListShare
    {
        public HubOrderListShare(OrderPrice price)
        {
            if (price == null)
                return;

            Surf = price.Cost;
            XPlay = price.XPlayShare;
            Ally = price.AllyShare;
        }

        public decimal Surf { get; set; }
        public decimal XPlay { get; set; }
        public decimal Ally { get; set; }
    }
}
