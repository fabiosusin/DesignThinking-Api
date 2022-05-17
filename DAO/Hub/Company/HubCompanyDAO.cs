using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.Company.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Hub.Company
{
    public class HubCompanyDAO : IBaseDAO<HubCompany>
    {
        internal RepositoryMongo<HubCompany> Repository;
        public HubCompanyDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(HubCompany obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubCompany obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubCompany obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubCompany obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubCompany FindOne() => Repository.FindOne();

        public HubCompany FindOne(Expression<Func<HubCompany, bool>> predicate) => Repository.FindOne(predicate);

        public HubCompany FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubCompany> Find(Expression<Func<HubCompany, bool>> predicate) => Repository.Collection.Find(Query<HubCompany>.Where(predicate));

        public IEnumerable<HubCompany> FindAll() => Repository.FindAll();
        
        public HubCompany GetDefaultCompany()
        {
            var company = FindOne(x => x.Default);
            if (company == null)
            {
                company = new HubCompany("XPlay Mobile e IOT", "60cc928efe1ad80af4783ec8");
                Insert(company);
            }

            return company;
        }
    }
}
