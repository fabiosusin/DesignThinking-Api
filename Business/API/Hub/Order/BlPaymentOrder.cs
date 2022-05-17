using Business.API.Hub.Integration.Asaas.Customer;
using DAO.DBConnection;
using DAO.Hub.Order;
using DTO.General.Base.Api.Output;
using DTO.Hub.Integration.Asaas.Enum;
using DTO.Hub.Order.Database;
using DTO.Hub.Order.Enum;
using DTO.Hub.Order.Output;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API.Hub.Order
{
    public class BlPaymentOrder
    {
        protected HubPaymentOrderDAO HubPaymentOrderDAO;
        protected BlAsaasCharge BlAsaasCharge;
        public BlPaymentOrder(XDataDatabaseSettings settings)
        {
            BlAsaasCharge = new(settings);
            HubPaymentOrderDAO = new(settings);
        }

        public BaseApiOutput SaveOrderAsaasCharges(string orderId, List<HubOrderCreationChargeOutput> charges)
        {
            if (string.IsNullOrEmpty(orderId))
                return new("Venda não informada");

            foreach (var charge in charges)
            {
                var resultInsert = HubPaymentOrderDAO.Insert(new(orderId, charge.Value, HubPaymentOrder.GetStatusFromAsaasStatus(charge.Status), BlAsaasCharge.GetAsaasData(charge)));
                if (!resultInsert.Success)
                    return new(resultInsert.Message);
            }

            return new(true);
        }

        public async Task<List<HubOrderCreationChargeOutput>> GetChargesList(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return null;

            var input = HubPaymentOrderDAO.FindByOrderId(orderId);
            if (!(input?.Any() ?? false))
                return null;

            var resultList = new List<HubOrderCreationChargeOutput>();
            foreach (var payment in input)
            {
                if (payment.AsaasData == null || payment.AsaasData.PaymentType == HubOrderPaymentFormEnum.Money)
                {
                    resultList.Add(new(HubOrderPaymentFormsOutput.Money, HubAsaasPaymentStatusEnum.Confirmed, payment.Value)
                    {
                        PaymentOrderId = payment.Id
                    });
                    continue;
                }

                if (string.IsNullOrEmpty(payment.AsaasData.AsaasId))
                {
                    resultList.Add(new(new("Não foi possível realizar a cobrança!"), HubPaymentOrder.GetPaymentString(payment.AsaasData.PaymentType), payment.Value)
                    {
                        PaymentOrderId = payment.Id
                    });
                    continue;
                }

                var asaasCharge = await BlAsaasCharge.GetChargeDetails(payment.AsaasData.AsaasId).ConfigureAwait(false);
                if (asaasCharge?.Charge != null)
                    _ = HubPaymentOrderDAO.UpdateAsaasData(payment.Id, asaasCharge.Charge);

                var result = await BlAsaasCharge.GetChargeOutput(asaasCharge, asaasCharge?.Charge?.BillingType).ConfigureAwait(false);
                if (result == null)
                    continue;

                result.Value = payment.Value;
                result.PaymentOrderId = payment.Id;
                resultList.Add(result);
            }

            return resultList;
        }
    }
}
