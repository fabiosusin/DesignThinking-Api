using DAO.DBConnection;
using DAO.General.Log;
using DAO.Hub.AccountPlan;
using DAO.Hub.AllyDAO;
using DAO.Hub.Company;
using DAO.Hub.Order;
using DAO.Hub.Product;
using DTO.Integration.Sige.Order.Output;
using DTO.Hub.Company.Database;
using DTO.Hub.Order.Database;
using DTO.Hub.Order.Input;
using Newtonsoft.Json;
using Services.Integration.Sige.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Useful.Service;
using DTO.General.Log.Enum;
using DTO.General.Log.Database;

namespace Business.API.Hub.Integration.Sige.Order
{
    public class BlSigeOrder
    {
        protected SigeOrderService SigeOrderService;
        protected LogHistoryDAO LogHistoryDAO;
        protected HubOrderDAO HubOrderDAO;
        protected HubAllyDAO HubAllyDAO;
        protected HubProductOrderDAO HubProductOrderDAO;
        protected HubProductDAO HubProductDAO;
        protected HubAccountPlanDAO HubAccountPlanDAO;
        protected HubCompanyDAO HubCompanyDAO;

        public BlSigeOrder(XDataDatabaseSettings settings)
        {
            HubProductDAO = new(settings);
            HubCompanyDAO = new(settings);
            HubAccountPlanDAO = new(settings);
            HubAllyDAO = new(settings);
            HubProductOrderDAO = new(settings);
            HubOrderDAO = new(settings);
            LogHistoryDAO = new(settings);
            SigeOrderService = new();
        }

        public async Task<SigeOrderApiOutput> CreateSigeOrder(HubOrderInput order, HubCompany company, List<HubProductOrderInput> products, string accountPlanName)
        {
            if (EnvironmentService.Get() == EnvironmentService.Dev)
                return new(true);

            try
            {
                if (order == null)
                    return new(false, "Dados da venda não informados");

                if (!(products?.Any() ?? false))
                    return new(false, "Dados dos produtos não informados");

                return await SigeOrderService.CreateOrder(new(order, company, accountPlanName, products)).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = "Erro ao adicionar venda no ERP!",
                    ExceptionMessage = e.Message,
                    Type = AppLogTypeEnum.XApiSigeRequestError,
                    Method = "CreateSigeOrder",
                    Data = JsonConvert.SerializeObject(order),
                    Date = DateTime.Now
                });
                return new(false, "Ocorreu um erro ao salvar a venda no ERP.");
            }
        }

        public async Task<SigeOrderApiOutput> CreateRecurrenceSigeOrder(string orderId)
        {
            var order = HubOrderDAO.FindById(orderId);
            if (order == null)
                return new("Venda não encontrada.");

            var ally = HubAllyDAO.FindById(order.AllyId);
            if (string.IsNullOrEmpty(ally?.Cnpj))
                return new("CNPJ do Aliado não informado.");

            var expenseAccountPlanId = ally.RecurrenceAccountPlanId;
            if (string.IsNullOrEmpty(expenseAccountPlanId))
                return new("Plano de Contas de recorrência não informado para o Aliado.");

            var accountPlan = HubAccountPlanDAO.FindById(expenseAccountPlanId);
            if (accountPlan == null)
                return new("Plano de Contas não encontrado.");

            var company = HubCompanyDAO.FindById(order.CompanyId);
            if (company == null)
                return new("Empresa não encontrada.");

            var newOrder = new HubOrder()
            {
                Customer= order.Customer,
                SellerId = order.SellerId,
                AllyId = order.AllyId,
                AccountPlanId = accountPlan.Id,
                CreationDate = DateTime.Now,
                Price = order.Price,
                Payments = order.Payments
            };

            var products = GetProductsOrderInput(orderId);
            if (!(products?.Any() ?? false))
                return new("Produtos da Venda não encontrados.");

            try
            {
                return await SigeOrderService.CreateOrder(new(newOrder, company, products, ally.Cnpj, accountPlan.Name)).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = "Erro ao adicionar venda no ERP!",
                    ExceptionMessage = e.Message,
                    Type = AppLogTypeEnum.XApiSigeRequestError,
                    Method = "CreateRecurrenceSigeOrder",
                    Data = JsonConvert.SerializeObject(order),
                    Date = DateTime.Now
                });
                return new(false, "Ocorreu um erro ao salvar a venda no ERP.");
            }
        }

        private List<HubProductOrderInput> GetProductsOrderInput(string orderId)
        {
            var order = HubOrderDAO.FindById(orderId);
            if (order == null)
                return null;

            var productsOrder = HubProductOrderDAO.GetProductsOrder(orderId);
            if (!(productsOrder?.Any() ?? false))
                return null;

            var result = new List<HubProductOrderInput>();
            foreach (var productOrder in productsOrder)
            {
                var product = HubProductDAO.FindById(productOrder.ProductId);
                if (product == null)
                    continue;

                result.Add(new(productOrder, product));
            }

            return result;
        }
    }
}
