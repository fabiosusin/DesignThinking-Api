using Business.API.Hub.Integration.Sige.Entry;
using DAO.DBConnection;
using DAO.General.Log;
using DAO.Hub.AccountPlan;
using DAO.Hub.AllyDAO;
using DAO.Hub.Company;
using DAO.Hub.Order;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Hub.Order.Enum;
using DTO.Hub.Order.Input;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Business.API.Hub.Entry
{
    public class BlEntry
    {
        protected LogHistoryDAO LogHistoryDAO;
        protected HubOrderDAO HubOrderDAO;
        protected HubCompanyDAO HubCompanyDAO;
        protected HubAllyDAO HubAllyDAO;
        protected HubAccountPlanDAO HubAccountPlanDAO;
        protected BlSigeEntry BlSigeEntry;
        public BlEntry(XDataDatabaseSettings settings)
        {
            BlSigeEntry = new(settings);
            HubAccountPlanDAO = new(settings);
            HubAllyDAO = new(settings);
            LogHistoryDAO = new(settings);
            HubOrderDAO = new(settings);
            HubCompanyDAO = new(settings);
        }

        public async Task CreateOrderEntry(string orderId, HubOrderEntryTypeEnum entryType)
        {
            var baseLogError = new AppLogHistory
            {
                Message = "Não foi possível realizar o lançamento de repasse no ERP!",
                Type = AppLogTypeEnum.XApiHubValidationError,
                Controller = "OrderController",
                Route = "create-order",
                Data = JsonConvert.SerializeObject(orderId),
                Method = "CreateSurfExpense",
                Date = DateTime.Now
            };

            if (entryType == HubOrderEntryTypeEnum.Unknown)
            {
                baseLogError.Message = "Tipo de Lançamento não informado!";
                LogHistoryDAO.Insert(baseLogError);
                return;
            }

            var order = HubOrderDAO.FindById(orderId);
            if (order == null)
            {
                baseLogError.Message = "Venda não encontrada!";
                LogHistoryDAO.Insert(baseLogError);
                return;
            }

            if (entryType == HubOrderEntryTypeEnum.AllyShare)
            {
                var isMasterAlly = HubAllyDAO.FindById(order.AllyId)?.IsMasterAlly ?? false;
                if (isMasterAlly)
                    return;
            }

            var companyName = HubCompanyDAO.FindById(order.CompanyId)?.Name;
            if (string.IsNullOrEmpty(companyName))
            {
                baseLogError.Message = "Empresa não encontrada!";
                LogHistoryDAO.Insert(baseLogError);
                return;
            }

            var price = 0m;
            var customerName = string.Empty;
            var accountPlanName = string.Empty;
            if (entryType == HubOrderEntryTypeEnum.AllyShare)
            {
                price = order.Price?.AllyShare ?? 0;
                var ally = HubAllyDAO.FindById(order.AllyId);
                customerName = ally?.Cnpj;
                accountPlanName = HubAccountPlanDAO.FindById(ally?.ExpenseAccountPlanId)?.Name;
            }
            else if (entryType == HubOrderEntryTypeEnum.SurfCost)
            {
                price = order.Price?.Cost ?? 0;
                accountPlanName = HubAccountPlanDAO.FindById(HubAllyDAO.FindOne(x => x.IsMasterAlly)?.ExpenseAccountPlanId)?.Name;

                // A Surf não é um aliado, porém para gerar a venda no Sige precisa de um cliente, com isso passa a Surf como 
                // aliado apenas para salvar o cliente ao gerar a venda no Sige
                var allySurf = new HubOrderAllyInput(string.Empty, "10455746000143");
                customerName = allySurf?.Cnpj;
            }

            if (price == 0)
                return;

            var resultTransfer = await BlSigeEntry.CreateEntry(price, $"Pedido: {order.Code}", companyName, customerName, accountPlanName).ConfigureAwait(false);
            if (!(resultTransfer?.Success ?? false))
            {
                if (!string.IsNullOrEmpty(resultTransfer?.Message))
                    baseLogError.Message = resultTransfer?.Message;

                LogHistoryDAO.Insert(baseLogError);
                return;
            }
        }
    }
}
