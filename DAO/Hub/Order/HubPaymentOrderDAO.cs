using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.Order.Database;
using DTO.Integration.Asaas.Payments.Output;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Hub.Order
{
    public class HubPaymentOrderDAO : IBaseDAO<HubPaymentOrder>
    {
        internal RepositoryMongo<HubPaymentOrder> Repository;
        public HubPaymentOrderDAO(IXDataDatabaseSettings settings)
        {
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(HubPaymentOrder obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubPaymentOrder obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubPaymentOrder obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubPaymentOrder obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubPaymentOrder FindOne() => Repository.FindOne();

        public HubPaymentOrder FindOne(Expression<Func<HubPaymentOrder, bool>> predicate) => Repository.FindOne(predicate);

        public HubPaymentOrder FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubPaymentOrder> Find(Expression<Func<HubPaymentOrder, bool>> predicate) => Repository.Collection.Find(Query<HubPaymentOrder>.Where(predicate));

        public IEnumerable<HubPaymentOrder> FindByOrderId(string orderId) => string.IsNullOrEmpty(orderId) ? null : Repository.Collection.Find(Query<HubPaymentOrder>.EQ(x => x.OrderId, orderId));

        public IEnumerable<HubPaymentOrder> FindAll() => Repository.FindAll();

        public DAOActionResultOutput UpdateAsaasData(string paymentId, AsaasCreateChargeOutput obj)
        {
            var payment = FindById(paymentId);
            if (payment?.AsaasData == null)
                return new("Dados do Pagamento não encontrado");

            _ = Repository.Collection.Update(Query<HubPaymentOrder>.EQ(x => x.Id, paymentId), 
                Update<HubPaymentOrder>.Set(x => x.AsaasData, 
                new OrderAsaasData(payment.AsaasData.PaymentType, obj.Id, obj.InvoiceUrl, obj.BankSlipUrl, obj.TransactionReceiptUrl)));
            return new(true);
        }
    }
}
