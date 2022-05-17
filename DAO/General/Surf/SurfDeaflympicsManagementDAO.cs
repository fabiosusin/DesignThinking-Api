using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.General.Surf.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAO.General.Surf
{
    public class SurfDeaflympicsManagementDAO : IBaseDAO<SurfDeaflympicsManagement>
    {
        internal RepositoryMongo<SurfDeaflympicsManagement> Repository;
        public SurfDeaflympicsManagementDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(SurfDeaflympicsManagement obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(SurfDeaflympicsManagement obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(SurfDeaflympicsManagement obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(SurfDeaflympicsManagement obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public SurfDeaflympicsManagement FindOne() => Repository.FindOne();

        public SurfDeaflympicsManagement FindOne(Expression<Func<SurfDeaflympicsManagement, bool>> predicate) => Repository.FindOne(predicate);

        public SurfDeaflympicsManagement FindById(string id) => Repository.FindById(id);

        public IEnumerable<SurfDeaflympicsManagement> Find(Expression<Func<SurfDeaflympicsManagement, bool>> predicate) => Repository.Collection.Find(Query<SurfDeaflympicsManagement>.Where(predicate));

        public IEnumerable<SurfDeaflympicsManagement> FindAll() => Repository.FindAll();
    }
}
