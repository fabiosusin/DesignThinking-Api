using DAO.DBConnection;
using DAO.Intra.Employee;
using DAO.Intra.EquipamentDAO;
using DAO.Intra.Loan;
using DAO.Intra.LoanHistory;
using DTO.Intra.Loan.Database;
using DTO.Intra.Loan.Input;
using DTO.Intra.Loan.Output;
using System;
using System.Linq;

namespace Business.API.Intra.Loan
{
    public class BlIntraLoan
    {
        private readonly IntraLoanDAO IntraLoanDAO;
        private readonly IntraEmployeeDAO IntraEmployeeDAO;
        private readonly IntraEquipmentDAO IntraEquipmentDAO;
        private readonly IntraEquipmentHistoryDAO IntraEquipmentHistoryDAO;

        public BlIntraLoan(XDataDatabaseSettings settings)
        {
            IntraLoanDAO = new(settings);
            IntraEmployeeDAO = new(settings);
            IntraEquipmentDAO = new(settings);
            IntraEquipmentHistoryDAO = new(settings);
        }

        public IntraLoanUpsertOutput UpsertLoan(IntraLoan input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.EmployeeId))
                return new("Funcionário não informado!");

            if (string.IsNullOrEmpty(input.UserId))
                return new("Usuário não informado!");

            if (input.LoanDate == DateTime.MinValue)
                return new("Data de empréstimo não informada!");

            if (input.EquipmentsIds.Select(x => IntraEquipmentDAO.FindById(x) == null).ToList().Find(x => x == true))
                return new("O equipamento não existe!");

            foreach (var id in input.EquipmentsIds)
            {
                var equipment = IntraEquipmentDAO.FindById(id);
                if (equipment == null)
                    return new("Equipamento não encontrado!");

                equipment.Loaned = true;
                IntraEquipmentDAO.Update(equipment);
            }

            var result = IntraLoanDAO.Upsert(input);
            if (result == null)
                return new("Não foi possível salvar o empréstimo!");

            return new(true, result.Data.Id);
        }

        public IntraLoanUpsertOutput EquipmentDevolution(IntraLoanDetails input)
        {
            var loan = IntraLoanDAO.FindById(input?.Id);
            if (loan == null)
                return new("Empréstimo não encontrado!");

            if (loan.Returned)
                return new("Devolução ja realizada!");

            if (loan.LoanDate > input.DevolutionDate)
                return new("Data da Devolução menor que a data do Empréstimo!");

            _ = IntraEquipmentDAO.UpdateReturnedEquipment(input.Equipments);

            var equipments = IntraEquipmentDAO.List(new(new(input.Equipments.Select(x => x.Id).ToList()))).ToList();
            if (equipments?.Count != loan.EquipmentsIds?.Count)
                return new("Equipamentos não encontrados para o Empréstimo!");

            loan.Returned = true;
            loan.DevolutionDate = input.DevolutionDate;

            equipments.ForEach(x => IntraEquipmentHistoryDAO.Insert(new(loan, x)));
            IntraLoanDAO.Update(loan);
            return new(true, loan.Id);
        }

        public IntraLoanDetailsOutput GetLoanDetails(string id)
        {
            var loan = IntraLoanDAO.FindById(id);
            if (loan == null)
                return new("Empréstimo não encontrado!");

            var equipments = IntraEquipmentDAO.List(new(new(loan.EquipmentsIds)));
            if (!(equipments?.Any() ?? false))
                return new("Equipamentos não encontrados para o Empréstimo!");

            var employeeName = IntraEmployeeDAO.FindById(loan.EmployeeId)?.Name;
            if (string.IsNullOrEmpty(employeeName))
                return new("Funcionário não encontrado!");

            return new(new IntraLoanDetails(loan, equipments, employeeName));
        }

        public IntraLoanListOutput List(IntraLoanListInput input)
        {
            var result = IntraLoanDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum empréstimo encontrado!");

            return new(result);
        }
    }
}
