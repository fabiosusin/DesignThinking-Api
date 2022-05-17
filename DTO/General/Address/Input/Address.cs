using System.Text.RegularExpressions;

namespace DTO.General.Address.Input
{
    public class Address
    {
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string CityCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public bool IsValid() => !string.IsNullOrEmpty(ZipCode) && ZipCodeIsValid(ZipCode) && !string.IsNullOrEmpty(Street) && !string.IsNullOrEmpty(Number)
            && !string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(Neighborhood) && !string.IsNullOrEmpty(Country);

        public bool ZipCodeIsValid(string self) => GetDigits(self)?.Length == 8;

        public string GetDigits(string self) =>
            self == null ? null : new Regex(@"[^\d]").Replace(self, "");
    }
}
