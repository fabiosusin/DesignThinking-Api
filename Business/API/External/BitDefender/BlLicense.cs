using DAO.DBConnection;
using DAO.External.BitDefender;
using DAO.General.Log;
using DAO.Hub.Product;
using DTO.External.BitDefender.Database;
using DTO.External.BitDefender.Input;
using DTO.General.Base.Api.Output;
using DTO.Hub.Menu.Enum;
using DTO.Hub.Product.Database;
using DTO.Hub.Product.Enum;
using System.Collections.Generic;

namespace Business.API.External.BitDefender
{
    public class BlLicense
    {
        private readonly HubProductDAO HubProductDAO;
        private readonly HubCategoryDAO HubCategoryDAO;
        private readonly BitDefenderLicenseDAO BitDefenderLicenseDAO;
        private readonly BitDefenderCategoryDAO BitDefenderCategoryDAO;
        public BlLicense(XDataDatabaseSettings settings)
        {
            HubProductDAO = new(settings);
            HubCategoryDAO = new(settings);
            BitDefenderLicenseDAO = new(settings);
            BitDefenderCategoryDAO = new(settings);
        }

        public List<BaseApiOutput> RegisterLicenses(List<BitDefenderLicenseDataInputApi> input)
        {
            var results = new List<BaseApiOutput>();
            foreach (var item in input)
            {
                var result = RegisterLicense(item);
                if (!result.Success)
                    result.Message += " Licença: " + item.Key;

                results.Add(result);
            }

            return results;
        }

        public BaseApiOutput RegisterLicense(BitDefenderLicenseDataInputApi input)
        {
            var baseValidation = BaseValidation(input);
            if (!(baseValidation?.Success ?? false))
                return baseValidation;

            if (BitDefenderLicenseDAO.FindOne(x => x.Key == input.Key) != null)
                return new("Licença já cadastrada com esta chave!");

            var categoryName = GetBitDefenderCategoryName(input.BitDefenderCategoryId);
            if (string.IsNullOrEmpty(categoryName))
                return new("Categoria não encontrada!");

            var category = BitDefenderCategoryDAO.FindOne(x => x.Name == categoryName);
            if (category == null)
                category = CreateCategory(categoryName);

            if (category == null)
                return new("Erro ao buscar a categoria da Licença!");

            var result = BitDefenderLicenseDAO.Insert(new(input, category.Id));
            if (!result.Success)
                return new(result.Message);

            return new(true);
        }

        private BitDefenderCategory CreateCategory(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return null;

            var categoryId = GetAntivirusCategoryId();
            if (string.IsNullOrEmpty(categoryId))
                return null;

            var category = (BitDefenderCategory)BitDefenderCategoryDAO.Insert(new(categoryName)).Data;
            if (category == null)
                return null;

            _ = HubProductDAO.Insert(new(category, categoryId));
            return category;
        }

        private string GetAntivirusCategoryId()
        {
            var bitDefenderCategory = new HubProductCategory("BitDefender", HubProductTypeEnum.Antivirus, new("fas fa-shield", HubIconTypeEnum.FontAwesome));
            var category = HubCategoryDAO.FindOne(x => x.Name == bitDefenderCategory.Name);
            if (category == null)
                category = (HubProductCategory)HubCategoryDAO.Insert(bitDefenderCategory).Data;

            return category?.Id;
        }

        private static string GetBitDefenderCategoryName(string key) => key?.ToLower() switch
        {
            "5763d259-a9f7-469b-a934-524206cfa39e" => "Av Plus",
            "ddb9d228-4f9c-4c3a-877d-2eca310c98ee" => "Total Security",
            _ => null
        };

        private static BaseApiOutput BaseValidation(BitDefenderLicenseDataInputApi input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.Key))
                return new("Chave não informada!");

            if (string.IsNullOrEmpty(input.BitDefenderCategoryId))
                return new("Id de Categoria não informada!");

            return new(true);
        }
    }
}
