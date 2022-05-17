using DAO.DBConnection;
using DAO.General.Log;
using DTO.General.Base.Api.Output;
using DTO.Integration.Sige.Customer.Output;
using DTO.Hub.Ally.Database;
using Newtonsoft.Json;
using Services.Integration.Sige.Customer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Useful.Service;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;

namespace Business.API.Hub.Integration.Sige.Customer
{
    public class BlSigeCustomer
    {
        protected SigeCustomerService SigeCustomerService;
        protected LogHistoryDAO LogHistoryDAO;
        public BlSigeCustomer(XDataDatabaseSettings settings)
        {
            LogHistoryDAO = new(settings);
            SigeCustomerService = new();
        }

        public async Task<BaseApiOutput> UpdateCustomer(HubAlly input)
        {
            if (EnvironmentService.Get() == EnvironmentService.Dev)
                return new(true);

            if (input == null)
                return new(false);

            try
            {
                return await SigeCustomerService.UpdateCustomer(new(input));
            }
            catch { return new(false); }
        }
        
        public async Task<List<SigeCustomerOutput>> GetCustomer(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            try
            {
                return await SigeCustomerService.GetCustomer(new(input));
            }
            catch { return null; }
        }
    }
}
