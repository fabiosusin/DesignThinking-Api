using DAO.DBConnection;
using DAO.General.Sequential;
using DAO.Hub.Company;
using DAO.Hub.CustomerDAO;
using DAO.Hub.Order;
using DTO.Hub.NFSe.Output;
using DTO.Hub.Order.Input;
using DTO.Hub.Order.Output;
using Services.Integration.NFSe.CaxiasDoSul;
using System.Threading.Tasks;

namespace Business.API.Hub.NFSe
{
    public class BlNfse
    {
        private readonly HubOrderDAO HubOrderDAO;
        private readonly HubCompanyDAO HubCompanyDAO;
        private readonly HubCustomerDAO HubCustomerDAO;
        private readonly SequentialCodeDAO SequentialCodeDAO;
        private readonly CaxiasDoSulSendNFSeService CaxiasDoSulSendNFSeService;
        private readonly CaxiasDoSulGetNFSeImageService CaxiasDoSulGetNFSeImageService;
        private readonly CaxiasDoSulGetNFSeStatusService CaxiasDoSulGetNFSeStatusService;
        private readonly CaxiasDoSulGetNFSeDetailsService CaxiasDoSulGetNFSeDetailsService;

        public BlNfse(XDataDatabaseSettings settings)
        {
            HubOrderDAO = new(settings);
            HubCompanyDAO = new(settings);
            HubCustomerDAO = new(settings);
            SequentialCodeDAO = new(settings);
            CaxiasDoSulSendNFSeService = new();
            CaxiasDoSulGetNFSeImageService = new();
            CaxiasDoSulGetNFSeStatusService = new();
            CaxiasDoSulGetNFSeDetailsService = new();
        }

        public async Task<HubOrderSendNfseOutput> GenerateCustomerNfse(string orderId)
        {
            var order = HubOrderDAO.FindById(orderId);
            if (order == null)
                return new("Venda não encontrada!");

            if (!string.IsNullOrEmpty(order.Nfse?.AccessKey))
                return new("Nota já emitida para esta venda!");

            var customer = HubCustomerDAO.FindById(order.Customer?.CustomerId);
            if (customer == null)
                return new("Cliente não encontrado!");

            var companyDetails = GetCompanyDetails(order.CompanyId);
            if (!companyDetails.Success)
                return new(companyDetails.Message);

            var number = SequentialCodeDAO.GetNfseCode(order.CompanyId);
            return new(await CaxiasDoSulSendNFSeService.SendNfse(
                new(number, order.Price.XPlayShare, new(companyDetails.Company), new(customer), companyDetails.Certified)).ConfigureAwait(false));
        }

        public async Task<HubNfseStatusOutput> GetNfseStatus(string orderId)
        {
            var order = HubOrderDAO.FindById(orderId);
            if (order == null)
                return new("Venda não encontrada!");
            
            if (string.IsNullOrEmpty(order.Nfse?.Lot))
                return new("Nenhuma nota foi emitida para esta venda!");

            var companyDetails = GetCompanyDetails(order.CompanyId);
            if (!companyDetails.Success)
                return new(companyDetails.Message);

            return new(await CaxiasDoSulGetNFSeStatusService.GetNfseStatus(new(companyDetails.Company.Cnpj, order.Nfse.Lot, companyDetails.Certified)).ConfigureAwait(false));
        }

        public async Task<HubNfseDetailsOutput> GetNfseDetails(string orderId)
        {
            var order = HubOrderDAO.FindById(orderId);
            if (order == null)
                return new("Venda não encontrada!");

            var companyDetails = GetCompanyDetails(order.CompanyId);
            if (!companyDetails.Success)
                return new(companyDetails.Message);

            return new(await CaxiasDoSulGetNFSeDetailsService.GetNfseDetails(new(companyDetails.Company.Cnpj, order.Nfse.AccessKey, companyDetails.Certified)).ConfigureAwait(false));
        }

        public async Task<HubNfseImageOutput> GetNfseImage(string orderId)
        {
            var order = HubOrderDAO.FindById(orderId);
            if (order == null)
                return new("Venda não encontrada!");

            var companyDetails = GetCompanyDetails(order.CompanyId);
            if (!companyDetails.Success)
                return new(companyDetails.Message);

            return new(await CaxiasDoSulGetNFSeImageService.GetNfseImage(new(companyDetails.Company.Cnpj, order.Nfse.AccessKey, companyDetails.Certified)).ConfigureAwait(false));
        }

        private HubNfseGetCompanyDetailsInput GetCompanyDetails(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
                return new("CompanyId não informado!");

            var company = HubCompanyDAO.FindById(companyId);
            if (company == null)
                return new("Empresa não encontrada!");

            var cert = GetCertified.Certified(company.Certified.Name, company.Certified.Password);
            if (cert == null)
                return new("Certificado não encontrado!");

            return new(company, cert);
        }

    }
}
