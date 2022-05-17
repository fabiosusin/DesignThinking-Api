using DTO.Hub.Cellphone.Input;
using DTO.Hub.Product.Database;
using System.Collections.Generic;

namespace DTO.Hub.Order.Input
{
    public class HubOrderRegisterCellphoneInput
    {
        public HubOrderRegisterCellphoneInput(HubOrderInput inputOrder, List<HubProductCellphoneInput> cellphoneProducts, List<HubProduct> listHubProducts, string orderId)
        {
            InputOrder = inputOrder;
            OrderId = orderId;
            CellphoneProducts = cellphoneProducts;
            ListHubProducts = listHubProducts;
        }

        public string OrderId { get; set; }
        public HubOrderInput InputOrder { get; set; }
        public List<HubProductCellphoneInput> CellphoneProducts { get; set; }
        public List<HubProduct> ListHubProducts { get; set; }
    }
}
