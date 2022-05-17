using DAO.DBConnection;
using DAO.Intra.Employee;
using DAO.Intra.Loan;
using DTO.General.Base.Api.Output;
using DTO.Intra.Employee.Database;
using DTO.Intra.Employee.Input;
using DTO.Intra.Employee.Output;
using System.Linq;
using Useful.Extensions;

namespace Business.API.Intra.Employee
{
    public class BlEmployee
    {
        private readonly IntraLoanDAO IntraLoanDAO;
        private readonly IntraEmployeeDAO IntraEmployeeDAO;

        public BlEmployee(XDataDatabaseSettings settings)
        {
            IntraLoanDAO = new(settings);
            IntraEmployeeDAO = new(settings);
        }

        public BaseApiOutput UpsertEmployee(IntraEmployee input)
        {
            var baseValidation = BasicValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            var result = string.IsNullOrEmpty(input.Id) ? IntraEmployeeDAO.Insert(input) : IntraEmployeeDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar o novo funcionário!") : new(true);
        }

        public IntraEmployee GetEmployee(string cpfCnpj) => string.IsNullOrEmpty(cpfCnpj) ? null : IntraEmployeeDAO.FindOne(x => x.CpfCnpj == cpfCnpj);

        public BaseApiOutput DeleteEmployee(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new("Requisição mal formada!");

            var employee = IntraEmployeeDAO.FindById(id);
            if (employee == null)
                return new("Funcionário não encontrado!");

            if (IntraLoanDAO.FindOne(x => x.EmployeeId == id) != null)
                return new("O funcionário possui um empréstimo vinculado!");

            IntraEmployeeDAO.Remove(employee);
            return new(true);
        }

        public IntraEmployeeListOutput List(IntraEmployeeListInput input)
        {
            var result = IntraEmployeeDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Funcionário encontrado!");

            return new(result);
        }

        private BaseApiOutput BasicValidation(IntraEmployee input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.Name))
                return new("Informe o Nome!");

            if (string.IsNullOrEmpty(input.CpfCnpj))
                return new("Informe o Cpf/Cnpj!");

            if (!input.CpfCnpj.IsCnpjOrCpf())
                return new("Cpf/Cnpj inválido!");

            if (IntraEmployeeDAO.FindOne(x => x.CpfCnpj == input.CpfCnpj && x.Id != input.Id) != null)
                return new("Já existe um funcionário cadastrado com este CPF/CNPJ");

            return new(true);
        }
    }
}
