using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Mobile.Account.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Mobile.Account
{
    public class AppOtpCodeDAO : IBaseDAO<AppOtpCode>
    {
        internal RepositoryMongo<AppOtpCode> Repository;
        public AppOtpCodeDAO(IXDataDatabaseSettings settings)
        {
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(AppOtpCode obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(AppOtpCode obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(AppOtpCode obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(AppOtpCode obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public AppOtpCode FindOne() => Repository.FindOne();

        public AppOtpCode FindOne(Expression<Func<AppOtpCode, bool>> predicate) => Repository.FindOne(predicate);

        public AppOtpCode FindById(string id) => Repository.FindById(id);

        public IEnumerable<AppOtpCode> Find(Expression<Func<AppOtpCode, bool>> predicate) => Repository.Collection.Find(Query<AppOtpCode>.Where(predicate));

        public IEnumerable<AppOtpCode> FindAll() => Repository.FindAll();

        public void DisableAllUserCodes(string mobileId)
        {
            if (string.IsNullOrEmpty(mobileId))
                return;

            var date = DateTime.Now;
            var codes = Repository.Find(x => x.AppMobileId == mobileId && (!x.Used || x.Expiration < date));
            if (!(codes?.Any() ?? false))
                return;

            foreach (var code in codes)
            {
                code.Used = true;
                Update(code);
            }
        }
    }
}
