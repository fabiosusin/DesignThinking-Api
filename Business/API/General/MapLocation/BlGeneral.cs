using DAO.DBConnection;
using DAO.General.Log;
using DTO.General.Address.Input;
using Services.Integration.ViaCep.Location;
using System;
using System.Threading.Tasks;

namespace Business.API.General.MapLocation
{
    public class BlGeneral
    {
        protected ViaCepGetLocationService ViaCepGetLocationService;
        protected LogHistoryDAO LogHistoryDAO;
        public BlGeneral(XDataDatabaseSettings settings)
        {
            LogHistoryDAO = new(settings);
            ViaCepGetLocationService = new();
        }

        public async Task<Address> GetAddressAsync(string zipCode)
        {
            try
            {
                var address = await ViaCepGetLocationService.GetAddress(zipCode).ConfigureAwait(false);
                return new Address
                {
                    ZipCode = zipCode,
                    City = address.Localidade,
                    CityCode = address.Ibge,
                    Complement = address.Complemento,
                    Neighborhood = address.Bairro,
                    State = address.UF,
                    Street = address.Logradouro,
                    Country = "Brasil"
                };
            }
            catch (Exception e)
            { 
                throw new Exception("O serviço não pode buscar o endereço do CEP: " + zipCode, e);
            }

        }
    }
}
