using Business.API.Hub.Integration.Sige.Customer;
using DAO.DBConnection;
using DAO.Hub.AccountPlan;
using DAO.Hub.AllyDAO;
using DTO.General.Base.Api.Output;
using DTO.Integration.Sige.Customer.Input;
using DTO.Hub.Ally.Database;
using System.Linq;
using System.Threading.Tasks;
using Useful.Extensions;
using DTO.Hub.Ally.Input;
using DTO.Hub.Ally.Output;
using DAO.Hub.Order;
using DAO.Hub.Cellphone;
using DAO.Hub.UserDAO;
using DAO.Hub.CustomerDAO;
using DAO.External.Visao;
using DAO.Hub.Application.Settings;
using DTO.Hub.Ally.Enum;

namespace Business.API.Hub.BlAlly
{
    public class BlAlly
    {
        private readonly VisaoCameraDAO VisaoCameraDAO;
        private readonly HubCustomerDAO HubCustomerDAO;
        private readonly HubUserDAO HubUserDAO;
        private readonly HubOrderDAO HubOrderDAO;
        private readonly HubAllyDAO HubAllyDAO;
        private readonly BlSigeCustomer BlSigeCustomer;
        private readonly HubAccountPlanDAO AccountPlanDAO;
        private readonly AppSettingsDAO AppSettingsDAO;
        private readonly HubCellphoneManagementDAO HubCellphoneManagementDAO;

        public BlAlly(XDataDatabaseSettings settings)
        {
            AppSettingsDAO = new(settings);
            VisaoCameraDAO = new(settings);
            HubCustomerDAO = new(settings);
            HubUserDAO = new(settings);
            HubCellphoneManagementDAO = new(settings);
            HubOrderDAO = new(settings);
            HubAllyDAO = new(settings);
            BlSigeCustomer = new(settings);
            AccountPlanDAO = new(settings);
        }

        public HubAllyDetailsOutput GetAlly(string allyId)
        {
            if (string.IsNullOrEmpty(allyId))
                return new("Id de Aliado não informado!");

            var ally = HubAllyDAO.FindById(allyId);
            return ally == null ? new("Aliado não encontrado!") : new(ally);
        }

        public async Task<HubAllyUpsertOutput> UpsertAlly(HubAlly input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.Name))
                return new("Informe o Nome do Aliado!");

            if (string.IsNullOrEmpty(input.AccountPlanId))
                return new("Informe um Plano de Conta!");

            if (string.IsNullOrEmpty(input.RecurrenceAccountPlanId))
                return new("Informe um Plano de Conta de Recorrência!");

            if (!input.Cnpj.IsCnpj())
                return new("Informe um CNPJ válido!");

            if (HubAllyDAO.FindOne(x => x.Name == input.Name && x.Id != input.Id) != null)
                return new("Já existe um Aliado com este mesmo Nome!");

            if (HubAllyDAO.FindOne(x => x.Cnpj == input.Cnpj && x.Id != input.Id) != null)
                return new("Já existe um Aliado com este mesmo CNPJ!");

            if (input.ChargeType == HubAllyChargeTypeEnum.Unknown)
                return new("Modo de Cobrança não informado!");

            if (AccountPlanDAO.FindOne(x => x.Id == input.AccountPlanId) == null)
                return new("Nenhum Plano de Conta encontrado a partir destes dados!");

            if (AccountPlanDAO.FindOne(x => x.Id == input.RecurrenceAccountPlanId) == null)
                return new("Nenhum Plano de Conta de Recorrência encontrado a partir destes dados!");

            if (string.IsNullOrEmpty(input.ExpenseAccountPlanId))
                return new("Informe um Plano de Conta de Despesa!");

            if (AccountPlanDAO.FindOne(x => x.Id == input.ExpenseAccountPlanId) == null)
                return new("Nenhum Plano de Conta de Despesa encontrado a partir destes dados!");

            if (!input.IsMasterAlly)
            {
                var newCustomer = new SigeCustomerInput(input);
                var sigeOutput = await BlSigeCustomer.UpdateCustomer(input).ConfigureAwait(false);
                if (!(sigeOutput?.Success ?? false))
                    return new(sigeOutput?.Message ?? "Não foi possível salvar o aliado no Sige!");
            }

            var result = string.IsNullOrEmpty(input.Id) ? HubAllyDAO.Insert(input) : HubAllyDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar um novo aliado!") : new(true, ((HubAlly)result.Data)?.Id);
        }

        public BaseApiOutput DeleteAlly(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new("Requisição mal formada!");

            var ally = HubAllyDAO.FindById(id);
            if (ally == null)
                return new("Aliado não encontrado!");

            if (HubOrderDAO.FindOne(x => x.AllyId == id) != null)
                return new("Aliado possui vendas vinculadas!");

            if (HubCustomerDAO.FindOne(x => x.AllyId == id) != null)
                return new("Aliado possui cliente vinculados!");

            if (HubUserDAO.FindOne(x => x.AllyId == id) != null)
                return new("Aliado possui usuários vinculados!");

            if (HubCellphoneManagementDAO.FindOne(x => x.AllyId == id) != null)
                return new("Aliado possui plano celular vinculado!");

            if (VisaoCameraDAO.FindOne(x => x.AllyId == id) != null)
                return new("Aliado possui câmeras do Visão360 vinculadas!");

            if (AppSettingsDAO.FindOne(x => x.AllyId == id) != null)
                return new("Aliado possui de Aplicativo vinculadas!");

            HubAllyDAO.Remove(ally);
            return new(true);
        }

        public HubAllyListOutput List(HubAllyListInput input)
        {
            var result = HubAllyDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Aliado encontrado!");

            return new(result);
        }
    }
}
