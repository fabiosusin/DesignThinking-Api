using System.Collections.Generic;

namespace DTO.Hub.Order.Input
{
    public class HubOrderCreationInput
    {
        public HubOrderCreationInput() { }
        public HubOrderCreationInput(HubOrderInput order, List<HubProductOrderInput> products)
        {
            Order = order;
            ProductsOrder = products;
        }

        public HubOrderInput Order { get; set; }
        public List<HubProductOrderInput> ProductsOrder { get; set; }
    }
}
