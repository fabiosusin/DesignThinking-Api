using DTO.Hub.Order.Database;
using DTO.Hub.Order.Input;
using System.Collections.Generic;
using System.Linq;

namespace DTO.Hub.Cellphone.Input
{
    public class HubCellphoneOrderCreation
    {
        public HubCellphoneOrderCreation() { }
        public HubCellphoneOrderCreation(HubOrder order, IEnumerable<HubProductCellphoneInput> products)
        {
            Order = new(order);
            ProductsOrder = products?.ToList();
        }

        public HubOrderCellphoneInput Order { get; set; }
        public List<HubProductCellphoneInput> ProductsOrder { get; set; }
    }

    public class HubOrderCellphoneInput
    {
        public HubOrderCellphoneInput(HubOrder order)
        {
            if (order == null)
                return;

            OrderId = order.Id;
            AllyId = order.AllyId;
            CustomerId = order.Customer?.CustomerId;
        }

        public string OrderId { get; set; }
        public string AllyId { get; set; }
        public string CustomerId { get; set; }
    }

    public class HubProductCellphoneInput
    {
        public HubProductCellphoneInput(HubProductOrder input, string name)
        {
            if (input == null)
                return;

            ProductOrderId = input.Id;
            CategoryId = input.CategoryId;
            CellphoneData = input.CellphoneData;
            OrderPrice = input.Price?.Price ?? 0;
            Name = name;
        }
        public string ProductOrderId { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public decimal OrderPrice { get; set; }
        public HubProductOrderCellphoneData CellphoneData { get; set; }
    }
}
