using DAO.Base;
using DAO.DBConnection;
using DAO.Intra.Employee;
using DAO.Intra.EquipamentDAO;
using DTO.General.DAO.Output;
using DTO.Intra.Employee.Database;
using DTO.Intra.Employee.Input;
using DTO.Intra.Equipament.Database;
using DTO.Intra.Loan.Database;
using DTO.Intra.Loan.Input;
using DTO.Intra.Loan.Output;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Intra.Loan
{
    public class IntraLoanDAO : IBaseDAO<IntraLoan>
    {
        internal RepositoryMongo<IntraLoan> Repository;
        private readonly IntraEmployeeDAO IntraEmployeeDAO;
        private readonly IntraEquipmentDAO IntraEquipmentDAO;

        public IntraLoanDAO(IXDataDatabaseSettings settings)
        {
            Repository = new(settings?.MongoDBSettings);
            IntraEmployeeDAO = new(settings);
            IntraEquipmentDAO = new(settings);
        }

        public DAOActionResultOutput Insert(IntraLoan obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(IntraLoan obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(IntraLoan obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(IntraLoan obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public IntraLoan FindOne() => Repository.FindOne();

        public IntraLoan FindOne(Expression<Func<IntraLoan, bool>> predicate) => Repository.FindOne(predicate);

        public IntraLoan FindById(string id) => Repository.FindById(id);

        public IEnumerable<IntraLoan> Find(Expression<Func<IntraLoan, bool>> predicate) => Repository.Collection.Find(Query<IntraLoan>.Where(predicate));

        public IEnumerable<IntraLoan> FindAll() => Repository.FindAll();

        public IEnumerable<IntraLoanListData> List(IntraLoanListInput input)
        {
            var loans = new List<IntraLoan>();
            if (input == null)
                loans = FindAll().ToList();
            else if (input.Paginator == null)
                loans = Repository.Collection.Find(GenerateFilters(input.Filters)).ToList();
            else
                loans = Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).ToList();

            if (!(loans?.Any() ?? false))
                return null;

            var employees = IntraEmployeeDAO.List(Fields<IntraEmployee>.Include(x => x.Name)).ToList();
            var equipments = IntraEquipmentDAO.ListWithFields(Fields<IntraEquipment>.Include(x => x.Name).Include(x => x.Code)).ToList();
            
            var result = new List<IntraLoanListData>();
            foreach (var loan in loans)
            {
                var employeeName = employees?.FirstOrDefault(x => x.Id == loan.EmployeeId)?.Name;
                if (string.IsNullOrEmpty(employeeName))
                    continue;

                var equipmentName = new List<string>();
                foreach (var equipmentId in loan.EquipmentsIds)
                {
                    var equipment = equipments.Find(x => x.Id == equipmentId);
                    equipmentName.Add(equipment.Code + " - " + equipment.Name);
                }

                result.Add( new(loan, employeeName, equipmentName));
            }

            return result;
        }

        public IMongoQuery GenerateFilters(IntraLoanFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var employees = IntraEmployeeDAO.List(new IntraEmployeeListInput { Filters = new IntraEmployeeFiltersInput { Name = input?.EmployeeName } }, Fields<IntraEmployee>.Include(x => x.Name)).ToList();

            var queryList = new List<IMongoQuery>();

            if (input.EquipmentIds?.Any() ?? false)
                queryList.Add(Query<IntraLoan>.In(x => x.Id, input.EquipmentIds));

            if (!string.IsNullOrEmpty(input.EmployeeId))
                queryList.Add(Query<IntraLoan>.EQ(x => x.EmployeeId, input.EmployeeId));

            if(input.DateFilterType == LoanDateFilterEnum.LoanDate)
            {
                if (input.StartDate.HasValue && input.StartDate != DateTime.MinValue)
                    queryList.Add(Query<IntraLoan>.GTE(x => x.LoanDate, input.StartDate.Value.Date));

                if (input.EndDate.HasValue && input.EndDate != DateTime.MinValue)
                    queryList.Add(Query<IntraLoan>.LTE(x => x.LoanDate, input.EndDate.Value.Date.AddDays(1).AddMilliseconds(-1)));

            } else if(input.DateFilterType == LoanDateFilterEnum.DevolutionDate)
            {
                if (input.StartDate.HasValue && input.StartDate != DateTime.MinValue)
                    queryList.Add(Query<IntraLoan>.GTE(x => x.DevolutionDate, input.StartDate.Value.Date));

                if (input.EndDate.HasValue && input.EndDate != DateTime.MinValue)
                    queryList.Add(Query<IntraLoan>.LTE(x => x.DevolutionDate, input.EndDate.Value.Date.AddDays(1).AddMilliseconds(-1)));
            }

            if (input.ReturnedFilterType == LoanReturnedFilterEnum.Returned)
                queryList.Add(Query<IntraLoan>.EQ(x => x.Returned, true));
            else if (input.ReturnedFilterType == LoanReturnedFilterEnum.NotReturned)
                queryList.Add(Query<IntraLoan>.EQ(x => x.Returned, false));

            if (!string.IsNullOrEmpty(input.EmployeeName) && (employees?.Any() ?? false))
                queryList.Add(Query<IntraLoan>.In(x => x.EmployeeId, employees.Select(x => x.Id)));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
