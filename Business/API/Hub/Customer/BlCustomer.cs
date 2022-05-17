using DAO.DBConnection;
using DAO.Hub.AllyDAO;
using DAO.Hub.Cellphone;
using DAO.Hub.CustomerDAO;
using DAO.Hub.Order;
using DTO.General.Base.Api.Output;
using DTO.Hub.Ally.Enum;
using DTO.Hub.Customer.Database;
using DTO.Hub.Customer.Enum;
using DTO.Hub.Customer.Input;
using DTO.Hub.Customer.Output;
using System.Linq;
using Useful.Extensions;
using static Utils.Extensions.Files.Images.ImageFactory;

namespace Business.API.Hub.BlCustomer
{
    public class BlCustomer
    {
        protected HubCustomerDAO CustomerDAO;
        protected HubCellphoneManagementDAO HubCellphoneManagementDAO;
        protected HubOrderDAO HubOrderDAO;
        protected HubAllyDAO AllyDAO;

        public BlCustomer(XDataDatabaseSettings settings)
        {
            HubCellphoneManagementDAO = new(settings);
            CustomerDAO = new(settings);
            AllyDAO = new(settings);
            HubOrderDAO = new(settings);
        }

        public BaseApiOutput UpsertCustomer(HubCustomer input)
        {
            var baseValidation = BasicValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            if (!string.IsNullOrEmpty(input.DocumentBase64))
                input.Document.ImageLink = SaveImageFromBase64(input.DocumentBase64, 500, GetImagesEnum.Jpeg)?.ImageJpeg;

            var result = string.IsNullOrEmpty(input.Id) ? CustomerDAO.Insert(input) : CustomerDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar o novo cliente!") : new(true);
        }

        public HubCustomer GetCustomer(string cpfCnpj, string allyId) => string.IsNullOrEmpty(cpfCnpj) || string.IsNullOrEmpty(allyId) ? null : CustomerDAO.FindOne(x => x.Document.Data == cpfCnpj && x.AllyId == allyId);

        public BaseApiOutput DeleteCustomer(string id, string allyId)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(allyId))
                return new("Requisição mal formada!");

            var customer = CustomerDAO.FindById(id);
            if (customer == null)
                return new("Cliente não encontrado!");

            if (customer.AllyId != allyId)
                return new("Cliente não pertence a este aliado!");

            if (HubOrderDAO.FindOne(x => x.Customer.CustomerId == id) != null)
                return new("Cliente possui vendas vinculadas!");

            if (HubCellphoneManagementDAO.FindOne(x => x.CustomerId == id) != null)
                return new("Cliente possui plano celular vinculado!");

            CustomerDAO.Remove(customer);
            return new(true);
        }

        public HubCustomerListOutput List(HubCustomerListInput input)
        {
            var result = CustomerDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Cliente encontrado!");

            return new(result);
        }

        private BaseApiOutput BasicValidation(HubCustomer input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.AllyId))
                return new("Id de Aliado não informado!");

            if (input.CellphoneData != null)
            {
                if (input.CellphoneData.Number.Length != 9)
                    return new("Informe o Número de Celular corretamente com 9 dígitos!");

                if (input.CellphoneData.DDD.Length != 2)
                    return new("Informe o DDD corretamente!");

                if (input.CellphoneData.CountryPrefix.Length != 2)
                    return new("Informe o Prefixo do País corretamente!");

                if (!long.TryParse(input.CellphoneData.Number, out _))
                    return new("Número de Celular não está em um formato correto!");

                if (!long.TryParse(input.CellphoneData.DDD, out _))
                    return new("DDD não está em um formato correto!");

                if (!long.TryParse(input.CellphoneData.CountryPrefix, out _))
                    return new("Prefixo do País não está em um formato correto!");
            }

            if (AllyDAO.FindById(input.AllyId) == null)
                return new("Aliado não encontrado!");

            if (string.IsNullOrEmpty(input.Email))
                return new("Informe o Email!");

            if (string.IsNullOrEmpty(input.Name))
                return new("Informe o Nome!");

            if (input.Document?.Type == HubDocumentTypeEnum.Unknown)
                return new("Informe o tipo de pessoa!");

            if (input.Document.Type == HubDocumentTypeEnum.Company)
            {
                if (!(input.Document.Data?.IsCnpj() ?? false))
                    return new("CNPJ informado não é válido!");

                if (input.IcmsType == HubIcmsTypeEnum.Unknown)
                    return new("Informe tipo de contribuição de ICMS!");
            }

            if (input.Document.Type == HubDocumentTypeEnum.Simple && !(input.Document.Data?.IsCpf() ?? false))
                return new("CPF informado não é válido!");

            if (string.IsNullOrEmpty(input.Id))
            {
                if (CustomerDAO.FindOne(x => x.Email == input.Email && x.AllyId == input.AllyId) != null)
                    return new("Usuário já cadastrado com este Email!");

                if (CustomerDAO.FindOne(x => x.Document.Data == input.Document.Data && x.AllyId == input.AllyId) != null)
                    return new("Usuário já cadastrado com este Documento!");
            }

            return new(true);
        }
    }
}
