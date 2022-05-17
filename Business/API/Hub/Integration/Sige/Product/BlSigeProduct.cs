using DAO.DBConnection;
using DAO.General.Log;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Integration.Sige.Product.Output;
using Services.Integration.Sige.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.API.Hub.Integration.Sige.Product
{
    public class BlSigeProduct
    {
        protected SigeProductService SigeProductService;
        protected LogHistoryDAO LogHistoryDAO;

        public BlSigeProduct(XDataDatabaseSettings settings)
        {
            LogHistoryDAO = new(settings);
            SigeProductService = new();
        }

        public async Task<IEnumerable<SigeProductInput>> GetProducts()
        {
            try
            {
                return await SigeProductService.GetProducts();
            }
            catch { return null; }
        }
    }
}
