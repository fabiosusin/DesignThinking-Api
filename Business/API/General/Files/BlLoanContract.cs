using Business.API.General.Files.Word;
using Business.API.Intra.Loan;
using DAO.DBConnection;
using DAO.Intra.Employee;
using DAO.Intra.EquipamentDAO;
using DAO.Intra.Loan;
using DTO.General.Files.Output;
using DTO.Intra.Equipament.Database;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Useful.Extensions;
using Useful.Service;

namespace Business.API.General.Files
{
    public class BlLoanContract : BlFileAbstract
    {
        private readonly BlIntraLoan BlIntraLoan;
        private readonly IntraLoanDAO IntraLoanDAO;
        private readonly IntraEmployeeDAO IntraEmployeeDAO;
        private readonly IntraEquipmentDAO IntraEquipmentDAO;
        private const string LoanDocumentPath = EnvironmentService.DocumentBasePath + "\\loan-contract.docx";
        private const string DevolutionDocumentPath = EnvironmentService.DocumentBasePath + "\\devolution-contract.docx";

        public BlLoanContract(XDataDatabaseSettings settings)
        {
            BlIntraLoan = new(settings);
            IntraLoanDAO = new(settings);
            IntraEmployeeDAO = new(settings);
            IntraEquipmentDAO = new(settings);
        }

        public override async Task<GenerateDocOutput> GenerateDoc(string id)
        {
            var loan = IntraLoanDAO.FindById(id);
            if (loan == null)
                return null;

            var isReturned = loan.Returned;

            using var myWebClient = new WebClient();
            byte[] bytes = myWebClient.DownloadData(isReturned ? DevolutionDocumentPath : LoanDocumentPath);
            if (bytes?.Length <= 0)
                return null;

            var file = $"{EnvironmentService.DocumentBasePath}\\{Guid.NewGuid()}.docx";

            var employee = IntraEmployeeDAO.FindById(loan.EmployeeId);
            var equipments = new List<IntraEquipment>();
            loan.EquipmentsIds.ForEach(x => equipments.Add(IntraEquipmentDAO.FindById(x)));

            using (var word = new OpenXMLWord(bytes))
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();

                if (!isReturned)
                {
                    dictionary = new Dictionary<string, string>
                    {
                        { "##name##", employee.Name },
                        { "##cpfcnpj##", employee.CpfCnpj.FormatCpfCnpj() },
                        { "##duedate##",  loan.LoanDate.ToString("dd/MM/yyyy") },
                        { "##contractdate##", StringExtension.GetPortugueseWrittenDate(loan.LoanDate) }
                    };
                } else
                {
                    dictionary = new Dictionary<string, string>
                    {
                        { "##name##", employee.Name },
                        { "##cpfcnpj##", employee.CpfCnpj.FormatCpfCnpj() },
                        { "##duedate##",  loan.LoanDate.ToString("dd/MM/yyyy") },
                        { "##contractdate##", StringExtension.GetPortugueseWrittenDate(loan.LoanDate) }
                    };
                }

                var replaces = dictionary;

                var equipmentsReplaces = new List<Dictionary<string, string>>();
                foreach (var equipment in equipments)
                {
                    equipmentsReplaces.Add(new Dictionary<string, string>
                    {
                        { "##equipmentname##", equipment.Name },
                        { "##equipmentnote##", equipment.Note },
                        { "##equipmentdamagenote##", equipment.DamageNote }
                    });
                }

                word.DoReplaces(replaces);
                word.DoTableReplaces(equipmentsReplaces);
                word.CloseDocument();
                word.SaveAs(file);
            }


            return isReturned ? new($"contratoDevolucao_{loan.Id}", WordWrapper.GeneratePdf(file)) : new($"contratoEmprestimo_{loan.Id}", WordWrapper.GeneratePdf(file));
        }

    }
}
