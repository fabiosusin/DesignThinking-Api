using DAO.DBConnection;
using DAO.Hub.CustomerDAO;
using DTO.Hub.Cellphone.Output;
using Services.Integration.Surf.Register.Customer;
using System;
using System.Threading.Tasks;

namespace Business.API.Hub.Integration.Surf.Customer
{
    public class BlSurfCustomer
    {
        private readonly HubCustomerDAO HubCustomerDAO;
        private readonly SurfCustomerService SurfCustomerService;
        public BlSurfCustomer(XDataDatabaseSettings settings)
        {
            SurfCustomerService = new();
            HubCustomerDAO = new(settings);
        }

        public async Task<HubSurfFindCustomerOutput> GetSurfCustomer(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new("Requisição mal formada!");

            var customer = HubCustomerDAO.FindById(input);
            if (customer == null)
                return new("Cliente não encontrado!");

            if (string.IsNullOrEmpty(customer.Document?.Data))
                return new("CPF/CNPJ não informado para o cliente!");

            if (string.IsNullOrEmpty(customer.CellphoneData?.Number))
                return new("Número telefônico não informado para o cliente!");

            if (string.IsNullOrEmpty(customer.CellphoneData?.DDD))
                return new("DDD não informado para o cliente!");

            if (string.IsNullOrEmpty(customer.CellphoneData.CountryPrefix))
                return new("Prefixo do País não informado!");

            try
            {
                _ = await SurfCustomerService.AddCustomer(new(customer)).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                var resultMessage = e.Message;
                if (string.IsNullOrEmpty(resultMessage) || (!resultMessage.Contains("CPF/CNPJ já cadastrado.") && !resultMessage.Contains("Código já cadastrado. Por favor, redefina-o.")))
                    return new(resultMessage ?? "Não foi possível salvar o cliente na Surf!");
            }

            try
            {
                var surfCustomer = await SurfCustomerService.GetCustomer(customer.Document.Data).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(surfCustomer?.Payload?.CustomerId))
                    return new(true, surfCustomer.Payload.CustomerId);

                return new("Cliente não encontrado na Surf!");
            }
            catch { }

            return new("Não foi possível buscar os dados do cliente na Surf!");
        }

    }
}
