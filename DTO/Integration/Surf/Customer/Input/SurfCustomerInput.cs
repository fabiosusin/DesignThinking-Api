using DTO.Hub.Customer.Database;
using System;
using System.Text;

namespace DTO.Integration.Surf.Input.Customer
{
    public class SurfCustomerInput
    {
        public SurfCustomerInput(HubCustomer customer)
        {
            if (customer == null)
                return;

            _ = int.TryParse(customer.CellphoneData?.DDD, out var ddd);
            Ddd = ddd;
            Name = customer.Name;
            Email = customer.Email;
            Document = customer?.Document?.Data;
            Code = RandomString(10);
            if (customer.Document != null)
                Phone = $"{customer.CellphoneData.CountryPrefix}{customer.CellphoneData.DDD}{customer.CellphoneData.Number}";
        }

        public string Document { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Ddd { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        private static string RandomString(int size)
        {
            var builder = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < size; i++)
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))));

            return builder.ToString().ToLower();
        }
    }

}
