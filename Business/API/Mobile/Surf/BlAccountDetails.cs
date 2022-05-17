using Business.API.Hub.Integration.Surf.CustomerMsisdn;
using DAO.DBConnection;
using DAO.General.Surf;
using DAO.Mobile.Account;
using DTO.General.Base.Api.Output;
using DTO.General.Surf.Database;
using DTO.General.Surf.Input;
using DTO.Integration.Surf.AccountDetails.Input;
using DTO.Integration.Surf.AccountDetails.Output;
using DTO.Surf.Enum;
using Services.Integration.Surf.Register.Customer;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Business.API.Mobile.Surf
{
    public class BlAccountDetails
    {
        private SurfAccountService SurfAccountService;
        private MobileAccountDAO MobileAccountDAO;
        private BlCustomerMsisdn BlCustomerMsisdn;
        private SurfCustomerMsisdnDAO SurfCustomerMsisdnDAO;
        public BlAccountDetails(XDataDatabaseSettings settings)
        {
            SurfAccountService = new(settings);
            BlCustomerMsisdn = new(settings);
            MobileAccountDAO = new(settings);
            SurfCustomerMsisdnDAO = new(settings);
        }

        public async Task<SurfAccountDetailsOutput> GetDetailsByCpf(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new SurfAccountDetailsOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "CPF não informado!"
                };

            input = input.GetDigits();
            return await SurfAccountService.GetAccountDetailsByCPF(new SurfAccountDetailsCpfInput(input)).ConfigureAwait(false);
        }

        public async Task<SurfAccountDetailsOutput> GetDetailsByMsisdn(string number, string countryPrefix, string allyId)
        {
            if (string.IsNullOrEmpty(number))
                return new SurfAccountDetailsOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "MSISDN não informado!"
                };

            number = number.GetDigits();
            countryPrefix = countryPrefix.GetDigits();
            var validNumber = BlCustomerMsisdn.ValidMSISDN(new(number, countryPrefix, allyId));
            if (!(validNumber?.Success ?? false))
            {
                return new SurfAccountDetailsOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = validNumber.Message
                };
            }

            return await SurfAccountService.GetAccountDetails(new SurfAccountDetailsInput(number)).ConfigureAwait(false);
        }

        public BaseApiOutput UpsertCustomerMsisdn(string number, string countryPrefix, string mobileId, string allyId, bool insert)
        {
            var baseValidation = BaseValidationCustomerMsisdn(number, countryPrefix, mobileId, allyId);
            if (!baseValidation.Success)
                return baseValidation;

            var account = MobileAccountDAO.FindOne(x => x.Cellphone == mobileId && x.AllyId == allyId);
            if (account == null)
                return new("Usuário não encontrado a partir destes dados!");

            var existing = SurfCustomerMsisdnDAO.FindOne(x => x.Number == number && x.NumberCountryPrefix == countryPrefix && x.AllyId == allyId);
            if (existing == null)
                return new("Número não encontrado a partir destes dados!");
            else
            {
                existing.AppCustomerId = insert ? account.Id : null;
                SurfCustomerMsisdnDAO.Update(existing);
            }

            return new(true);
        }

        private static BaseApiOutput BaseValidationCustomerMsisdn(string number, string countryPrefix, string email, string allyId)
        {
            if (string.IsNullOrEmpty(number))
                return new("MSISDN não informado!");

            if (string.IsNullOrEmpty(countryPrefix))
                return new("MSISDN CountryPrefix não informado!");

            if (string.IsNullOrEmpty(email))
                return new("Email não informado!");

            if (string.IsNullOrEmpty(allyId))
                return new("Id de aliado não informado!");

            return new(true);
        }
    }
}
