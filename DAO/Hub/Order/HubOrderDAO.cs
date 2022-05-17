using DAO.DBConnection;
using DTO.Hub.Order.Database;
using MongoDB.Driver.Builders;
using DTO.Hub.Order.Input;
using DTO.Hub.Order.Output;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using DAO.Hub.AccountPlan;
using DAO.Hub.UserDAO;
using DAO.Hub.CustomerDAO;
using DTO.Hub.User.Database;
using DTO.Hub.Customer.Database;
using DTO.Hub.AccountPlan.Database;
using DTO.Hub.Customer.Input;
using DTO.Hub.AccountPlan.Input;
using DTO.Hub.User.Input;
using System;
using DAO.Base;
using System.Linq.Expressions;
using DTO.Hub.Order.Enum;
using DTO.General.DAO.Output;
using DTO.General.Log.Enum;
using DAO.General.Sequential;

namespace DAO.Hub.Order
{
    public class HubOrderDAO : IBaseDAO<HubOrder>
    {
        private readonly HubUserDAO HubUserDAO;
        internal RepositoryMongo<HubOrder> Repository;
        private readonly HubCustomerDAO HubCustomerDAO;
        private readonly SequentialCodeDAO SequencialCodeDAO;
        private readonly HubAccountPlanDAO HubAccountPlanDAO;
        public HubOrderDAO(IXDataDatabaseSettings settings)
        {
            HubUserDAO = new(settings);
            HubCustomerDAO = new(settings);
            SequencialCodeDAO = new(settings);
            HubAccountPlanDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(HubOrder order)
        {
            if (order == null)
                return new("Objeto não informado");

            order.Code = SequencialCodeDAO.GetNextCode(SequentialCodeTypeEnum.HubOrder);
            var result = Repository.Insert(order);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput UpdateNFSeData(string objId, HubOrderNfse data)
        {
            Repository.Collection.Update(Query<HubOrder>.EQ(x => x.Id, objId), Update<HubOrder>.Set(x => x.Nfse, data));
            return new(true);
        }

        public DAOActionResultOutput UpdateStatus(string objId, HubOrderStatusEnum status)
        {
            Repository.Collection.Update(Query<HubOrder>.EQ(x => x.Id, objId), Update<HubOrder>.Set(x => x.Status, status));
            return new(true);
        }

        public DAOActionResultOutput UpdateSigeCode(string objId, long sigeCode)
        {
            Repository.Collection.Update(Query<HubOrder>.EQ(x => x.Id, objId), Update<HubOrder>.Set(x => x.SigeCode, sigeCode));
            return new(true);
        }

        public DAOActionResultOutput Update(HubOrder obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubOrder obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubOrder obj)
        {
            RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            var order = FindById(id);
            if (order == null)
                return new("Venda não encontrada");

            SequencialCodeDAO.RollbackCode(SequentialCodeTypeEnum.HubOrder, order.Code);
            Repository.RemoveById(id);
            return new(true);
        }

        public HubOrder FindOne() => Repository.FindOne();

        public HubOrder FindOne(Expression<Func<HubOrder, bool>> predicate) => Repository.FindOne(predicate);

        public HubOrder FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubOrder> Find(Expression<Func<HubOrder, bool>> predicate) => Repository.Collection.Find(Query<HubOrder>.Where(predicate));

        public IEnumerable<HubOrder> FindAll() => Repository.FindAll();

        public IEnumerable<HubOrder> FindByListIds(IEnumerable<string> ids) => ids?.Any() ?? false ? Repository.Collection.Find(Query<HubOrder>.In(y => y.Id, ids)) : null;

        public DAOActionResultOutput UpdateOrderStatus(string orderId, HubOrderStatusEnum status)
        {
            if (string.IsNullOrEmpty(orderId))
                return new("Id da venda não informado!");

            var order = FindById(orderId);
            if (order == null)
                return new("Venda não encontrada!");

            order.Status = status;
            _ = Update(order);

            return new(true);
        }

        public decimal TotalOrder(string allyId = null) => string.IsNullOrEmpty(allyId) ? Repository.Collection.Count() : Repository.Collection.Count(Query<HubOrder>.EQ(x => x.AllyId, allyId));

        public IEnumerable<HubOrderListData> List(HubOrderListInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            var queryList = new List<IMongoQuery>();

            var customers = HubCustomerDAO.List(new HubCustomerListInput { Filters = new HubCustomerFiltersInput { Name = input.Filters?.CustomerName } }, Fields<HubCustomer>.Include(x => x.Name)).ToList();
            var sellers = HubUserDAO.List(new HubUserListInput { Filters = new HubUserFiltersInput { Name = input.Filters?.SellerName } }, Fields<HubUser>.Include(x => x.Name)).ToList();
            var accountPlans = HubAccountPlanDAO.List(new HubAccountPlanListInput { Filters = new HubAccountPlanFiltersInput(input.Filters?.AccountPlanName) }, Fields<HubAccountPlan>.Include(x => x.Name)).ToList();

            if (input.Filters != null)
            {
                if (!string.IsNullOrEmpty(input.Filters.CustomerName) && (customers?.Any() ?? false))
                    queryList.Add(Query<HubOrder>.In(x => x.Customer.CustomerId, customers.Select(x => x.Id)));

                if (!string.IsNullOrEmpty(input.Filters.SellerName) && (sellers?.Any() ?? false))
                    queryList.Add(Query<HubOrder>.In(x => x.SellerId, sellers.Select(x => x.Id)));

                if (!string.IsNullOrEmpty(input.Filters.AccountPlanName) && (accountPlans?.Any() ?? false))
                    queryList.Add(Query<HubOrder>.In(x => x.AccountPlanId, accountPlans.Select(x => x.Id)));

                if (input.Filters.StartDate.HasValue && input.Filters.StartDate != DateTime.MinValue)
                    queryList.Add(Query<HubOrder>.GTE(x => x.CreationDate, input.Filters?.StartDate.Value.Date));

                if (input.Filters.EndDate.HasValue && input.Filters.EndDate != DateTime.MinValue)
                    queryList.Add(Query<HubOrder>.LTE(x => x.CreationDate, input.Filters?.EndDate.Value.Date.AddDays(1).AddMilliseconds(-1)));

                if (!string.IsNullOrEmpty(input.Filters.AllyId))
                    queryList.Add(Query<HubOrder>.EQ(x => x.AllyId, input.Filters.AllyId));

                if (input.Filters.Ids?.Any() ?? false)
                    queryList.Add(Query<HubOrder>.In(x => x.Id, input.Filters.Ids));

                if (input.Filters.Status != HubOrderStatusEnum.Unknown)
                    queryList.Add(Query<HubOrder>.EQ(x => x.Status, input.Filters.Status));
            }

            var orders = new List<HubOrder>();
            if (input == null)
                orders = FindAll().OrderByDescending(x => x.CreationDate).ToList();
            else if (input.Paginator == null)
                orders = Repository.Collection.Find(queryList.Any() ? Query.And(queryList) : emptyResult).SetSortOrder(SortBy<HubOrder>.Descending(x => x.CreationDate)).ToList();
            else
                orders = Repository.Collection.Find(queryList.Any() ? Query.And(queryList) : emptyResult).SetSortOrder(SortBy<HubOrder>.Descending(x => x.CreationDate)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).ToList();

            if (!(orders?.Any() ?? false))
                return null;

            var result = new List<HubOrderListData>();
            foreach (var order in orders)
            {
                var custormerName = customers?.FirstOrDefault(x => x.Id == order.Customer?.CustomerId)?.Name;
                if (string.IsNullOrEmpty(custormerName))
                    continue;

                var sellerName = sellers?.FirstOrDefault(x => x.Id == order.SellerId)?.Name;
                if (string.IsNullOrEmpty(sellerName))
                    continue;

                var accountPlanName = accountPlans?.FirstOrDefault(x => x.Id == order.AccountPlanId)?.Name;
                if (string.IsNullOrEmpty(accountPlanName))
                    continue;

                result.Add(new HubOrderListData(order)
                {
                    CustomerName = custormerName,
                    SellerName = sellerName,
                    AccountPlanName = accountPlanName
                });
            }

            return result;
        }
    }
}
