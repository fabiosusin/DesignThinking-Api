using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAO.Hub.Cellphone;
using DTO.General.Invoice.Input;
using DTO.General.Invoice.Database;

namespace DAO.General.Invoice
{
    public class InvoiceCustomerDAO : IBaseDAO<InvoiceCustomer>
    {
        private readonly IXDataDatabaseSettings Settings;
        internal RepositoryMongo<InvoiceCustomer> Repository;
        public InvoiceCustomerDAO(IXDataDatabaseSettings settings)
        {
            Settings = settings;
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(InvoiceCustomer obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(InvoiceCustomer obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(InvoiceCustomer obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(InvoiceCustomer obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public InvoiceCustomer FindOne() => Repository.FindOne();

        public InvoiceCustomer FindOne(Expression<Func<InvoiceCustomer, bool>> predicate) => Repository.FindOne(predicate);

        public InvoiceCustomer FindById(string id) => Repository.FindById(id);

        public DAOActionResultOutput PaidInvoice(string id)
        {
            Repository.Collection.Update(Query<InvoiceCustomer>.EQ(x => x.Id, id), Update<InvoiceCustomer>.Set(x => x.Paid, true).Set(x => x.PayIn, DateTime.Now));
            return new(true);
        }

        public IEnumerable<InvoiceCustomer> Find(Expression<Func<InvoiceCustomer, bool>> predicate) => Repository.Collection.Find(Query<InvoiceCustomer>.Where(predicate));

        public IEnumerable<InvoiceCustomer> FindAll() => Repository.FindAll();

        public IEnumerable<InvoiceCustomer> List(InvoiceCustomerListInput input) => input == null ? Repository.Collection.FindAll().SetSortOrder(SortBy<InvoiceCustomer>.Descending(x => x.CreationDate)) : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)).SetSortOrder(SortBy<InvoiceCustomer>.Descending(x => x.CreationDate)) :
            Repository.Collection.Find(GenerateFilters(input.Filters)).SetSortOrder(SortBy<InvoiceCustomer>.Descending(x => x.CreationDate)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

        private IMongoQuery GenerateFilters(InvoiceCustomerFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.AllyId))
                queryList.Add(Query<InvoiceCustomer>.EQ(x => x.AllyId, input.AllyId));

            if (!string.IsNullOrEmpty(input.HubCustomerId))
                queryList.Add(Query<InvoiceCustomer>.EQ(x => x.HubCustomerId, input.HubCustomerId));

            if (input.Paid.HasValue)
                queryList.Add(Query<InvoiceCustomer>.EQ(x => x.Paid, input.Paid));

            if (input.CellphonesManagementIds?.Any() ?? false)
                queryList.Add(Query<InvoiceCustomer>.In(x => x.CellphoneManagementId, input.CellphonesManagementIds));

            if (input.StartDate.HasValue)
                queryList.Add(Query<InvoiceCustomer>.GTE(x => x.ExpirationDate, input.StartDate));

            if (input.EndDate.HasValue)
                queryList.Add(Query<InvoiceCustomer>.LTE(x => x.ExpirationDate, input.EndDate));

            if (!string.IsNullOrEmpty(input.Number))
            {
                var cellphone = new HubCellphoneManagementDAO(Settings).GetCellphoneByNumber(input.Number);
                queryList.Add(Query<InvoiceCustomer>.EQ(x => x.CellphoneManagementId, cellphone?.Id));
                queryList.Add(Query<InvoiceCustomer>.EQ(x => x.HubCustomerId, cellphone?.CustomerId));
            }

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
