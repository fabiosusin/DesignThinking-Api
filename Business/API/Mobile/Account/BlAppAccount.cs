using DAO.DBConnection;
using DAO.General.Surf;
using DAO.Mobile.Account;
using DTO.General.Base.Api.Output;
using DTO.Mobile.Account.Database;
using DTO.Mobile.Account.Input;
using DTO.Mobile.Account.Output;

namespace Business.API.Mobile.Account
{
    public class BlAppAccount
    {
        private readonly MobileAccountDAO MobileAccountDAO;
        private readonly SurfCustomerMsisdnDAO SurfCustomerMsisdnDAO;
        public BlAppAccount(XDataDatabaseSettings settings)
        {
            MobileAccountDAO = new(settings);
            SurfCustomerMsisdnDAO = new(settings);
        }

        public AppLoginOutput Login(AppLoginInput input)
        {
            var result = FindAccountByMobileId(input);
            if (result == null)
                return result;

            var number = SurfCustomerMsisdnDAO.FindOne(x => x.Number == input.MobileId);
            if (number != null)
            {
                number.AppCustomerId = result.Id;
                SurfCustomerMsisdnDAO.Update(number);
            }

            return result;
        }

        public AppLoginOutput FindAccountByMobileId(AppLoginInput input)
        {
            if (string.IsNullOrEmpty(input?.MobileId))
                return new("Número de Celular não informado!");

            if (string.IsNullOrEmpty(input?.AllyId))
                return new("Aliado não informado!");

            var account = MobileAccountDAO.FindOne(x => x.Cellphone == input.MobileId && x.AllyId == input.AllyId);
            return account == null ? null : new(account.Id, account.Name, account.Cellphone, account.CellphoneCountryPrefix);
        }

        public AppLoginOutput AddAccount(AppAddAccountInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.AllyId))
                return new("Id de Aliado não informado!");

            if (MobileAccountDAO.FindOne(x => x.Cellphone == input.Cellphone && x.AllyId == input.AllyId) != null)
                return new("Usuário já cadastrado no sistema");

            if (string.IsNullOrEmpty(input.CellphoneCountryPrefix))
                return new("Informe o prefixo do seu país no número de Celular");

            if (input.CellphoneCountryPrefix.Length > 3)
                return new("Informe prefixo do seu país no número de Celular corretamente com até 3 dígitos");

            if (string.IsNullOrEmpty(input.Cellphone))
                return new("Informe seu número de Celular");

            if (input.Cellphone.Length != 11)
                return new("Informe seu número de Celular corretamente com DDD mais 9 dígitos");

            AppCustomerAccount result = null;
            var resultInsert = MobileAccountDAO.Insert(new AppCustomerAccount
            {
                Name = input.Name,
                AllyId = input.AllyId,
                Cellphone = input.Cellphone,
                CellphoneCountryPrefix = input.CellphoneCountryPrefix
            });

            if (resultInsert.Success)
                result = (AppCustomerAccount)resultInsert.Data;

            return result == null ? null : new(result.Id.ToString(), result.Name, result.Cellphone, result.CellphoneCountryPrefix);
        }

        public BaseApiOutput UpdateAccount(AppUpdateAccountDataInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.UserId))
                return new("Id de usuário não informado!");

            var account = MobileAccountDAO.FindOne(x => x.Id == input.UserId);
            if (account == null)
                return new("Usuário não encontrado!");

            if (string.IsNullOrEmpty(input.Name))
                return new("Nome não informado!");

            account.Name = input.Name;
            _ = MobileAccountDAO.Update(account);

            return new(true);
        }
    }
}
