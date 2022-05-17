using DTO.Integration.Sige.Order.Enum;
using DTO.Hub.Company.Database;
using DTO.Hub.Order.Database;
using DTO.Hub.Order.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO.Hub.Order.Enum;

namespace DTO.Integration.Sige.Order.Input
{
    public class SigeOrderInput
    {
        public SigeOrderInput(HubOrderInput order, HubCompany company, string accountPlanName, List<HubProductOrderInput> products)
        {
            var sigeProducts = new List<SigeProductOrderInput>();
            foreach (var product in products)
                sigeProducts.Add(new SigeProductOrderInput(product));

            var orderPrice = sigeProducts.Select(x => x.ValorUnitario).Sum(x => x);
            DepositoId = order.DepositId;
            PlanoDeConta = accountPlanName;
            Empresa = company.Name;
            ClienteCnpj = order.Ally?.Cnpj;
            FormaPagamento = HubPaymentOrder.GetPaymentString(order.Payments?.FirstOrDefault()?.Type ?? HubOrderPaymentFormEnum.Unknown);
            StatusSistema = SigeOrderStatusEnum.OrderInvoiced;
            ValorFinal = orderPrice;
            Items = sigeProducts;
            DataAprovacaoPedido = DateTime.Now.ToString();
            Pagamentos = order.Payments.Select(x=> new SigePaymentOrderInput(HubPaymentOrder.GetPaymentString(x.Type), x.Value, false)).ToList();
        }

        public SigeOrderInput(HubOrder order, HubCompany company, List<HubProductOrderInput> products, string customerDocument, string accountPlanName)
        {
            var sigeProducts = new List<SigeProductOrderInput>();
            foreach (var product in products)
                sigeProducts.Add(new SigeProductOrderInput(product));

            var orderPrice = sigeProducts.Select(x => x.ValorUnitario).Sum(x => x);
            DepositoId = company.DefaultDepositId;
            PlanoDeConta = accountPlanName;
            Empresa = company.Name;
            ClienteCnpj = customerDocument;
            FormaPagamento = HubPaymentOrder.GetPaymentString(order.Payments?.FirstOrDefault()?.Type ?? HubOrderPaymentFormEnum.Unknown);
            StatusSistema = SigeOrderStatusEnum.OrderInvoiced;
            ValorFinal = orderPrice;
            Items = sigeProducts;
            DataAprovacaoPedido = DateTime.Now.ToString();
            Pagamentos = order.Payments.Select(x => new SigePaymentOrderInput(HubPaymentOrder.GetPaymentString(x.Type), x.Value, false)).ToList();
        }

        public string StatusSistema { get; set; }
        public string DataAprovacaoPedido { get; set; }
        public string Cliente { get; set; }
        public string ClienteCnpj { get; set; }
        public string Empresa { get; set; }
        public string FormaPagamento { get; set; }
        public decimal ValorFinal { get; set; }
        public string DepositoId { get; set; }
        public string PlanoDeConta { get; set; }
        public string Descricao { get; set; }
        public List<SigeProductOrderInput> Items { get; set; }
        public List<SigePaymentOrderInput> Pagamentos { get; set; }
    }
}
