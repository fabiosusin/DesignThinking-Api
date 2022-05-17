using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.General.Log.Enum;
using DTO.General.SequentialCode.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.General.Sequential
{
    public class SequentialCodeDAO : IBaseDAO<SequentialCode>
    {
        internal RepositoryMongo<SequentialCode> Repository;
        public SequentialCodeDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(SequentialCode obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(SequentialCode obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(SequentialCode obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(SequentialCode obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public SequentialCode FindOne() => Repository.FindOne();

        public SequentialCode FindOne(Expression<Func<SequentialCode, bool>> predicate) => Repository.FindOne(predicate);

        public SequentialCode FindById(string id) => Repository.FindById(id);

        public IEnumerable<SequentialCode> Find(Expression<Func<SequentialCode, bool>> predicate) => Repository.Collection.Find(Query<SequentialCode>.Where(predicate));

        public IEnumerable<SequentialCode> FindAll() => Repository.FindAll();

        public void RollbackCode(SequentialCodeTypeEnum type, long code)
        {
            var sequential = FindByType(type);
            if (sequential.Code != code)
                return;

            sequential.Code = sequential.Code == 1 ? 0 : sequential.Code - 1;
            _ = Update(sequential);
        }

        public long GetNextCode(SequentialCodeTypeEnum type) => DefaultSequentialReturn(FindByType(type));

        public long GetNfseCode(string companyId) => DefaultSequentialReturn(FindByDataId(companyId));

        private long DefaultSequentialReturn(SequentialCode sequential)
        {
            sequential.Code++;
            _ = Update(sequential);
            return sequential.Code;
        }

        private SequentialCode FindByType(SequentialCodeTypeEnum type)
        {
            if (type == SequentialCodeTypeEnum.Unknown)
                return null;

            var existing = FindOne(x => x.Type == type);
            if (existing == null)
            {
                existing = new(type);
                _ = Insert(existing);
            }

            return existing;
        }

        private SequentialCode FindByDataId(string dataId)
        {
            if (string.IsNullOrEmpty(dataId))
                return null;

            var existing = FindOne(x => x.DataId == dataId);
            if (existing == null)
            {
                existing = new(dataId, SequentialCodeTypeEnum.HubCompanyNfse);
                _ = Insert(existing);
            }

            return existing;
        }
    }
}
