using DAO.DBConnection;
using DAO.General.Surf;
using DTO.General.Base.Api.Output;
using DTO.General.Surf.Database;
using DTO.General.Surf.Input;

namespace Business.API.Hub.Integration.Surf.CustomerMsisdn
{
    public class BlCustomerMsisdn
    {
        protected SurfCustomerMsisdnDAO SurfCustomerMsisdnDAO;
        public BlCustomerMsisdn(XDataDatabaseSettings settings) => SurfCustomerMsisdnDAO = new(settings);

        public BaseApiOutput ValidMSISDN(SurfCustomerMsisdnInput input)
        {
            var baseValidation = BaseValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            var data = SurfCustomerMsisdnDAO.FindOne(x => x.Number == input.Number && x.NumberCountryPrefix == input.CountryPrefix && x.AllyId == input.AllyId);
            return data == null ? new("Número não cadastrado no nosso sistema!") : new(true);
        }

        public BaseApiOutput InsertCustomerMsisdn(SurfCustomerMsisdnInput input)
        {
            var baseValidation = BaseValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            if (SurfCustomerMsisdnDAO.FindOne(x => x.Number == input.Number && x.NumberCountryPrefix == input.CountryPrefix) != null)
                return new("Msisdn já cadastrado!");

            var result = SurfCustomerMsisdnDAO.Insert(new SurfCustomerMsisdn(input));
            return result.Success ? new(true) : new(result.Message);
        }

        public BaseApiOutput RemoveCustomerMsisdn(string countryPrefix, string msisdn)
        {
            if (string.IsNullOrEmpty(msisdn))
                return new("Msisdn não informado!");

            var data = SurfCustomerMsisdnDAO.FindOne(x => x.Number == msisdn && x.NumberCountryPrefix == countryPrefix);
            if (data == null)
                return new("Msisdn não cadastrado!");

            SurfCustomerMsisdnDAO.Remove(data);
            return new(true);
        }

        private static BaseApiOutput BaseValidation(SurfCustomerMsisdnInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.AllyId))
                return new("Id de Aliado não informado!");

            if (string.IsNullOrEmpty(input.Number))
                return new("Msisdn não informado!");

            return new(true);
        }
    }
}
