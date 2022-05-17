using DAO.DBConnection;
using DAO.External.BitDefender;
using DAO.Hub.Order;
using DTO.External.BitDefender.Database;
using DTO.General.Base.Api.Output;
using DTO.Hub.BitDefender.Input;
using DTO.Hub.BitDefender.Output;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.API.Hub.BitDefender
{
    public class BlLicense
    {
        private readonly BitDefenderLicenseDAO BitDefenderLicenseDAO;
        private readonly BitDefenderCategoryDAO BitDefenderCategoryDAO;
        public BlLicense(XDataDatabaseSettings settings)
        {
            BitDefenderLicenseDAO = new(settings);
            BitDefenderCategoryDAO = new(settings);
        }

        public HubBitDefenderLicensesListOutput List(HubBitDefenderLicensesListInput input)
        {
            var result = BitDefenderLicenseDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhuma Licença encontrada!");

            return new(result);
        }

        public BaseApiOutput MarkLicensesAsUsed(IEnumerable<BitDefenderUseLicensesInput> licensesInput, string orderId, string allyId)
        {
            if (!(licensesInput?.Any() ?? false))
                return new("Licenças não informadas!");

            if (licensesInput?.Any(x => string.IsNullOrEmpty(x.CategoryId)) ?? false)
                return new("Informe a categoria das licenças!");

            if (licensesInput?.Any(x => x.Quantity <= 0) ?? false)
                return new("Informe a quantidade das licenças corretamente!");

            if (string.IsNullOrEmpty(orderId))
                return new("Id de venda não informado!");

            if (string.IsNullOrEmpty(allyId))
                return new("Id de aliado não informado!");

            var licensesToUpdate = new List<BitDefenderLicense>();
            foreach (var licenseInput in licensesInput)
            {
                var category = BitDefenderCategoryDAO.FindById(licenseInput.CategoryId);
                if (category == null)
                    return new("Categoria não encontrada!");

                var licenses = BitDefenderLicenseDAO.GetAvailableLicenses(licenseInput.CategoryId, licenseInput.Quantity);
                if (licenses?.Count() < licenseInput.Quantity)
                    return new($"Licenças {category.Name}, insuficientes para compra!");

                foreach (var license in licenses)
                {
                    license.Used = true;
                    license.AllyId = allyId;
                    license.OrderId = orderId;
                    license.LastUpdate = DateTime.Now;
                    licensesToUpdate.Add(license);
                }
            }

            foreach (var license in licensesToUpdate)
                _ = BitDefenderLicenseDAO.Update(license);

            return new(true);
        }

        public BitDefenderUseLicensesOutput GetUseLicensesOutput(string orderId, string categoryId)
        {
            var category = BitDefenderCategoryDAO.FindById(categoryId);
            if (category == null)
                return new("Categoria não encontrada!");

            var licenses = BitDefenderLicenseDAO.Find(x => x.OrderId == orderId && x.BitDefenderCategoryId == categoryId);
            if (!(licenses?.Any() ?? false))
                return new("Licenças não encontradas!");

            return new(new BitDefenderLicensesOutput(category.Id, category.Name, licenses.Select(x => x.Key)));
        }

        public BitDefenderCategory GetBitDefenderLicenseCategory(string licenseId) => string.IsNullOrEmpty(licenseId) ? null : BitDefenderCategoryDAO.FindById(BitDefenderLicenseDAO.FindById(licenseId)?.BitDefenderCategoryId);

        public IEnumerable<BitDefenderCategory> GetCategories() => BitDefenderCategoryDAO.FindAll();
    }
}
