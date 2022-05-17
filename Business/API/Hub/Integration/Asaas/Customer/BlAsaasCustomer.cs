using DAO.DBConnection;
using DAO.General.Log;
using DAO.Hub.CustomerDAO;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Integration.Asaas.Customer.Input;
using DTO.Integration.Asaas.Customer.Output;
using Newtonsoft.Json;
using Services.Integration.Asaas.Customer;
using System;
using System.Threading.Tasks;

namespace Business.API.Hub.Integration.Asaas.Customer
{
    public class BlAsaasCustomer
    {
        protected AsaasCustomerService AsaasCustomerService;
        protected LogHistoryDAO LogHistoryDAO;
        protected HubCustomerDAO HubCustomerDAO;
        public BlAsaasCustomer(XDataDatabaseSettings settings)
        {
            LogHistoryDAO = new(settings);
            HubCustomerDAO = new(settings);
            AsaasCustomerService = new();
        }

        public async Task<AsaasGetCustomerOutput> GetCustomerById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new("Id do Cliente não informado");

            var customer = HubCustomerDAO.FindById(id);
            if (customer == null)
                return new("Cliente não encontrado");

            var result = string.IsNullOrEmpty(customer.AsaasId) ? await CreateCustomer(id).ConfigureAwait(false) : await GetCustomerByAsaasId(customer.AsaasId).ConfigureAwait(false);
            if (result == null)
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = $"Não foi possível {(string.IsNullOrEmpty(customer.AsaasId) ? "cadastra" : "buscar")} o Cliente no Asaas!",
                    Type = AppLogTypeEnum.XApiHubValidationError,
                    Controller = "OrderController",
                    Route = "create-order",
                    Data = JsonConvert.SerializeObject(customer),
                    Method = "CreateSurfExpense",
                    Date = DateTime.Now
                });
            }

            return result;
        }

        private async Task<AsaasGetCustomerOutput> GetCustomerByAsaasId(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new("Id do Cliente não informado");

            try
            {
                return await AsaasCustomerService.GetCustomer(input);
            }
            catch { return new("Não foi possível buscar o Cliente"); }
        }

        private async Task<AsaasGetCustomerOutput> CreateCustomer(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new("Id do Cliente não informado");

            var customer = HubCustomerDAO.FindById(input);
            if (customer == null)
                return new("Cliente não encontrado");

            if (string.IsNullOrEmpty(customer.Name))
                return new("Nome do Cliente não informado");

            if (string.IsNullOrEmpty(customer.Document?.Data))
                return new("CPF/CNPJ do Cliente não informado");

            try
            {
                var result = await AsaasCustomerService.CreateCustomer(new(customer));
                if (!string.IsNullOrEmpty(result?.Customer?.Id))
                    HubCustomerDAO.UpdateAsaasId(customer.Id, result.Customer.Id);

                return result;
            }
            catch { return new("Não foi cadastrar buscar o Cliente"); }
        }
    }
}
