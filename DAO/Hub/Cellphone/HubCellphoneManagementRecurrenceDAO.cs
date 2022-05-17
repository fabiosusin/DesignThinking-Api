using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.Cellphone.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Hub.Cellphone
{
    public class HubCellphoneManagementRecurrenceDAO : IBaseDAO<HubCellphoneManagementRecurrence>
    {
        internal RepositoryMongo<HubCellphoneManagementRecurrence> Repository;
        public HubCellphoneManagementRecurrenceDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(HubCellphoneManagementRecurrence obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubCellphoneManagementRecurrence obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubCellphoneManagementRecurrence obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubCellphoneManagementRecurrence obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubCellphoneManagementRecurrence FindOne() => Repository.FindOne();

        public HubCellphoneManagementRecurrence FindOne(Expression<Func<HubCellphoneManagementRecurrence, bool>> predicate) => Repository.FindOne(predicate);

        public HubCellphoneManagementRecurrence FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubCellphoneManagementRecurrence> Find(Expression<Func<HubCellphoneManagementRecurrence, bool>> predicate) => Repository.Collection.Find(Query<HubCellphoneManagementRecurrence>.Where(predicate));

        public IEnumerable<HubCellphoneManagementRecurrence> FindAll() => Repository.FindAll();
        public IEnumerable<HubCellphoneManagementRecurrence> FindByListManagementId(IEnumerable<string> ids) => ids?.Any() ?? false ? Repository.Collection.Find(Query<HubCellphoneManagementRecurrence>.In(y => y.CellphoneManagementId, ids)).SetSortOrder(SortBy<HubCellphoneManagementRecurrence>.Descending(x => x.CreationDate)) : null;
    }
}
