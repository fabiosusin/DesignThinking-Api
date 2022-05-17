using Business.API.General.Files.Word;
using Business.API.Hub.Order;
using DAO.DBConnection;
using DTO.General.Files.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Useful.Extensions;
using Useful.Service;

namespace Business.API.General.Files
{
    public class BlOrderContract : BlFileAbstract
    {
        private const string DocumentPath = EnvironmentService.DocumentBasePath + "\\mvno-contract.docx";
        private readonly BlOrder BlOrder;
        public BlOrderContract(XDataDatabaseSettings settings)
        {
            BlOrder = new(settings);
        }

        public override async Task<GenerateDocOutput> GenerateDoc(string id)
        {
            var orderDetails = await BlOrder.GetOrderDetails(id).ConfigureAwait(false);
            if (orderDetails?.Order?.Customer == null)
                return null;

            using var myWebClient = new WebClient();
            byte[] bytes = myWebClient.DownloadData(DocumentPath);
            if (bytes?.Length <= 0)
                return null;

            var file = $"{EnvironmentService.DocumentBasePath}\\{Guid.NewGuid()}.docx";
            var order = orderDetails.Order;
            var products = orderDetails.ProductsOrder;

            using (var word = new OpenXMLWord(bytes))
            {
                var price = order.Payments?.Sum(x => x.Value).ToMoney();
                var replaces = new Dictionary<string, string>
                    {
                        { "##name##", order.Customer.Name },
                        { "##cpfcnpj##", order.Customer.Document?.Data?.FormatCpfCnpj() },
                        { "##birthday##", order.Customer.BirhDay == DateTime.MinValue ? "": order.Customer.BirhDay.ToString("dd/MM/yyyy") },
                        { "##phone##", order.Customer.CellphoneData?.CellphoneMask() },
                        { "##email##", order.Customer.Email },
                        { "##duedate##",  order.CreationDate.ToString("dd/MM/yyyy") },
                        { "##paymenttype##",  string.Join(", ", order.Payments?.Select(x=> x.PaymentForm)) },
                        { "##chipprice##",  products.Sum(x=> x.Price.ChipPrice).ToMoney() },
                        { "##price##",  order.Payments?.Sum(x => x.Value).ToMoney() },
                        { "##validytime##", "1 mês" },
                        { "##contractdate##", StringExtension.GetPortugueseWrittenDate(order.CreationDate) }
                    };

                if (order.Customer.Address?.IsValid() ?? false)
                {
                    replaces.Add("##address##", $"Rua: {order.Customer.Address.Street} Nº{order.Customer.Address.Number} - {order.Customer.Address.Neighborhood}, {order.Customer.Address.ZipCode}");
                    replaces.Add("##citystate##", $"{order.Customer.Address.City}/{order.Customer.Address.State}");
                }

                var productsReplaces = new List<Dictionary<string, string>>();
                foreach (var product in products)
                {
                    productsReplaces.Add(new Dictionary<string, string>
                    {
                        { "##servicename##", product.Name },
                        { "##serviceprice##", product.Price?.Price.ToMoney() }
                    });
                }

                word.DoReplaces(replaces);
                word.DoTableReplaces(productsReplaces);
                word.CloseDocument();
                word.SaveAs(file);
            }

            return new($"contratoVenda_{order.Code}", WordWrapper.GeneratePdf(file));
        }

    }
}
