using DAO.DBConnection;
using DAO.Hub.CustomerDAO;
using DTO.Hub.Cellphone.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using DTO.Hub.Cellphone.Input;
using DTO.Hub.Cellphone.Output;
using DTO.Hub.Customer.Database;
using DTO.Hub.Customer.Input;
using MongoDB.Driver;
using System.Linq;
using DAO.Hub.Order;
using DAO.Base;
using DTO.General.DAO.Output;
using System.Linq.Expressions;
using DTO.Hub.Cellphone.Enum;
using DAO.General.Invoice;

namespace DAO.Hub.Cellphone
{
    public class HubCellphoneManagementDAO : IBaseDAO<HubCellphoneManagement>
    {
        private readonly HubOrderDAO HubOrderDAO;
        private readonly HubCustomerDAO HubCustomerDAO;
        private readonly InvoiceCustomerDAO InvoiceCustomerDAO;
        private readonly HubCellphoneManagementRecurrenceDAO HubCellphoneManagementRecurrenceDAO;
        internal RepositoryMongo<HubCellphoneManagement> Repository;
        public HubCellphoneManagementDAO(IXDataDatabaseSettings settings)
        {
            HubOrderDAO = new(settings);
            HubCustomerDAO = new(settings);
            InvoiceCustomerDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
            HubCellphoneManagementRecurrenceDAO = new(settings);
        }

        public DAOActionResultOutput Insert(HubCellphoneManagement obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubCellphoneManagement obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubCellphoneManagement obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubCellphoneManagement obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubCellphoneManagement FindOne() => Repository.FindOne();

        public HubCellphoneManagement FindOne(Expression<Func<HubCellphoneManagement, bool>> predicate) => Repository.FindOne(predicate);

        public HubCellphoneManagement FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubCellphoneManagement> FindByOrderId(string id) => Repository.Find(x => x.OrderId == id);

        public IEnumerable<HubCellphoneManagement> Find(Expression<Func<HubCellphoneManagement, bool>> predicate) => Repository.Collection.Find(Query<HubCellphoneManagement>.Where(predicate));

        public IEnumerable<HubCellphoneManagement> FindAll() => Repository.FindAll();

        public HubCellphoneManagement GetCellphoneByNumber(string mobileId)
        {
            if (string.IsNullOrEmpty(mobileId))
                return null;

            var cellphoneData = new SurfCellphoneData(mobileId);
            if (string.IsNullOrEmpty(cellphoneData?.Number))
                return null;

            if (!string.IsNullOrEmpty(cellphoneData?.CountryPrefix) && !string.IsNullOrEmpty(cellphoneData?.DDD))
                return Repository.Collection.FindOne(Query.And(
                    Query<HubCellphoneManagement>.EQ(x => x.CellphoneData.CountryPrefix, cellphoneData.CountryPrefix),
                    Query<HubCellphoneManagement>.EQ(x => x.CellphoneData.DDD, cellphoneData.DDD),
                    Query<HubCellphoneManagement>.EQ(x => x.CellphoneData.Number, cellphoneData.Number)));

            if (!string.IsNullOrEmpty(cellphoneData?.DDD))
                return Repository.Collection.FindOne(Query.And(
                    Query<HubCellphoneManagement>.EQ(x => x.CellphoneData.DDD, cellphoneData.DDD),
                    Query<HubCellphoneManagement>.EQ(x => x.CellphoneData.Number, cellphoneData.Number)));

            return Repository.Collection.FindOne(Query<HubCellphoneManagement>.EQ(x => x.CellphoneData.Number, cellphoneData.Number));
        }

        public IEnumerable<HubCellphoneManagement> GetRecurrence(DateTime input) => Repository.Collection.Find(Query.And(
                Query.Or(
                    Query<HubCellphoneManagement>.EQ(x => x.Status, HubCellphoneManagementStatusEnum.Defaulter),
                    Query<HubCellphoneManagement>.EQ(x => x.Status, HubCellphoneManagementStatusEnum.AwaitingChargePayment),
                    Query<HubCellphoneManagement>.EQ(x => x.Status, HubCellphoneManagementStatusEnum.Completed)),
                Query<HubCellphoneManagement>.GTE(x => x.RecurrenceDate, input.Date),
                Query<HubCellphoneManagement>.LTE(x => x.RecurrenceDate, input.Date.AddDays(1).AddMilliseconds(-1)),
                Query<HubCellphoneManagement>.LT(x => x.LastUpdate, input)));

        public IEnumerable<HubCellphoneManagement> GetDateRecurrence(DateTime date)
        {
            var now = DateTime.Now.Date;
            return Repository.Collection.Find(Query.And(
                Query.Or(
                    Query<HubCellphoneManagement>.EQ(x => x.Status, HubCellphoneManagementStatusEnum.Defaulter),
                    Query<HubCellphoneManagement>.EQ(x => x.Status, HubCellphoneManagementStatusEnum.Completed)),
                Query<HubCellphoneManagement>.GTE(x => x.RecurrenceDate, date.Date),
                Query<HubCellphoneManagement>.LTE(x => x.RecurrenceDate, date.Date.AddDays(1).AddMilliseconds(-1)),
                Query<HubCellphoneManagement>.LT(x => x.LastUpdate, now)));
        }

        public IEnumerable<HubRecurrenceListData> List(HubRecurrenceListInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            var queryList = new List<IMongoQuery>();

            var customers = HubCustomerDAO.List(new HubCustomerListInput { Filters = new HubCustomerFiltersInput { Name = input.Filters?.CustomerName } }, Fields<HubCustomer>.Include(x => x.Name)).ToList();
            var numbers = new List<HubCellphoneManagement>();
            if (input == null)
                numbers = FindAll().OrderByDescending(x => x.CreationDate).ToList();
            else if (input.Paginator == null)
                numbers = Repository.Collection.Find(GenerateFilters(input.Filters, customers)).SetSortOrder(SortBy<HubCellphoneManagement>.Descending(x => x.CreationDate)).ToList();
            else
                numbers = Repository.Collection.Find(GenerateFilters(input.Filters, customers)).SetSortOrder(SortBy<HubCellphoneManagement>.Descending(x => x.CreationDate)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).ToList();

            if (!(numbers?.Any() ?? false))
                return null;

            var orders = HubOrderDAO.FindByListIds(numbers.Select(x => x.OrderId));
            var ids = numbers.Select(x => x.Id);
            var recurrences = HubCellphoneManagementRecurrenceDAO.FindByListManagementId(ids);
            var invoices = InvoiceCustomerDAO.List(new(new(ids)));

            var result = new List<HubRecurrenceListData>();
            foreach (var number in numbers)
            {
                var customerName = customers?.FirstOrDefault(x => x.Id == number.CustomerId)?.Name;
                if (string.IsNullOrEmpty(customerName))
                    continue;

                result.Add(new HubRecurrenceListData
                {
                    Id = number.Id,
                    CustomerName = customerName,
                    Recurrences = recurrences?.Where(x => x.CellphoneManagementId == number.Id).Select(x => new HubRecurrence(x.CreationDate, x.Status)),
                    Invoices = invoices?.Where(x => x.CellphoneManagementId == number.Id).Select(x => new HubInvoiceCustomer(x)),
                    CreationDate = number.CreationDate,
                    Status = number.Status,
                    Mode = number.Mode,
                    OrderPrice = number.Price?.OrderPrice ?? 0,
                    SurfPrice = number.Price?.SurfPlanPrice ?? 0,
                    Number = !string.IsNullOrEmpty(number.CellphoneData?.Number) && !string.IsNullOrEmpty(number.CellphoneData?.DDD) ?
                number.CellphoneData.DDD + number.CellphoneData.Number : !string.IsNullOrEmpty(number.CellphoneData?.Number) ? number.CellphoneData.Number : null
                });
            }

            return result;
        }

        private static IMongoQuery GenerateFilters(HubRecurrenceFiltersInput input, List<HubCustomer> customers)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.CustomerName) && (customers?.Any() ?? false))
                queryList.Add(Query<HubCellphoneManagement>.In(x => x.CustomerId, customers.Select(x => x.Id)));

            if (input.StartDate.HasValue && input.StartDate != DateTime.MinValue)
                queryList.Add(Query<HubCellphoneManagement>.GTE(x => x.CreationDate, input?.StartDate.Value.Date));

            if (input.EndDate.HasValue && input?.EndDate != DateTime.MinValue)
                queryList.Add(Query<HubCellphoneManagement>.LTE(x => x.CreationDate, input?.EndDate.Value.Date.AddDays(1).AddMilliseconds(-1)));

            if (!string.IsNullOrEmpty(input.AllyId))
                queryList.Add(Query<HubCellphoneManagement>.EQ(x => x.AllyId, input.AllyId));

            if (!string.IsNullOrEmpty(input.CellphoneData?.CountryPrefix) && !string.IsNullOrEmpty(input.CellphoneData?.DDD) && !string.IsNullOrEmpty(input.CellphoneData?.Number))
                queryList.Add(Query<HubCellphoneManagement>.EQ(x => x.CellphoneData, input.CellphoneData));
            else if (!string.IsNullOrEmpty(input.CellphoneData?.DDD) && !string.IsNullOrEmpty(input.CellphoneData?.Number))
                queryList.Add(Query.And(Query<HubCellphoneManagement>.EQ(x => x.CellphoneData.DDD, input.CellphoneData.DDD), Query<HubCellphoneManagement>.EQ(x => x.CellphoneData.Number, input.CellphoneData.Number)));
            else if (!string.IsNullOrEmpty(input.CellphoneData?.Number))
                queryList.Add(Query<HubCellphoneManagement>.EQ(x => x.CellphoneData.Number, input.CellphoneData.Number));

            if (input.Type != HubCellphoneManagementTypeEnum.Unknown)
                queryList.Add(Query<HubCellphoneManagement>.EQ(x => x.Mode, input.Type));

            if (input.Status != HubCellphoneManagementStatusEnum.Unknown)
                queryList.Add(Query<HubCellphoneManagement>.EQ(x => x.Status, input.Status));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
