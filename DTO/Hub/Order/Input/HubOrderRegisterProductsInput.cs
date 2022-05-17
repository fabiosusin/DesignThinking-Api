using System.Collections.Generic;

namespace DTO.Hub.Order.Input
{
    public class HubOrderRegisterProductsInput
    {
        public HubOrderRegisterProductsInput(List<HubProductOrderInput> input, string orderId)
        {
            Products = input;
            OrderId = orderId;
        }

        public string OrderId { get; set; }
        public List<HubProductOrderInput> Products { get; set; }
    }
}
