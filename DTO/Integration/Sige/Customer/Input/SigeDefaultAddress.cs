using DTO.General.Address.Input;

namespace DTO.Integration.Sige.Customer.Input
{
    public class SigeDefaultAddress
    {
        public SigeDefaultAddress(Address input)
        {
            if (input == null)
                return;

            Exterior = !string.IsNullOrEmpty(input.Country) && input.Country.ToLower() != "brasil";
            Logradouro = input.Street;
            Numero = input.Number;
            Complemento = input.Complement;
            Bairro = input.Neighborhood;
            Cidade = input.City;
            Pais = input.Country;
            CEP = input.ZipCode;
            Uf = input.State;
        }

        public bool Exterior { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Uf { get; set; }
        public string CodigoUF { get; set; }
        public string Cidade { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string CodigoCidade { get; set; }
        public string Pais { get; set; }
        public string CodigoPais { get; set; }
    }
}
