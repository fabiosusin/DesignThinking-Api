using DAO.DBConnection;
using DAO.Mobile.Account;
using DAO.Mobile.Visao;
using DTO.General.Pagination.Input;
using DTO.Mobile.Visao.Enum;
using DTO.Mobile.Visao.Input;
using DTO.Mobile.Visao.Output;
using System.Collections.Generic;
using System.Linq;

namespace Business.API.Mobile.Visao
{
    public class BlCameras : BlBase
    {
        public BlCameras(XDataDatabaseSettings settings) : base(settings) { }

        public IEnumerable<AppCameraListOutput> GetCamerasList(AppCameraListInput input)
        {
            if (string.IsNullOrEmpty(input?.Filters?.AllyId))
                return new List<AppCameraListOutput>() { new AppCameraListOutput("Id de Aliado não informado!") };

            var cameras = VisaoCameraDAO.Find(input?.Filters);
            if (!(cameras?.Any() ?? false))
                return null;

            var paginator = input?.Paginator ?? new PaginatorInput(1, 4);

            if (paginator.Page <= 0)
                paginator.Page = 1;

            if (paginator.ResultsPerPage <= 0)
                paginator.ResultsPerPage = 4;

            var result = new List<AppCameraListOutput>();
            foreach (var item in cameras.GroupBy(x => x.Address.City))
            {
                var first = item.FirstOrDefault();
                var groupCameras = item.Skip((paginator.Page - 1) * paginator.ResultsPerPage).Take(paginator.ResultsPerPage).ToList();
                result.Add(new AppCameraListOutput(first.Address.City, first.Address.State, item.Count(), groupCameras.Select(x => new AppCameraOutput(x)).ToList()));
            }

            return result.OrderByDescending(x => x.CamerasOnline);
        }

        public AppCameraDetailsOutput GetCameraDetails(string id, string mobileId, string allyId, bool otherWithFreeAccess)
        {
            if (string.IsNullOrEmpty(allyId))
                return new AppCameraDetailsOutput("Id de Aliado não informado!");

            var camera = string.IsNullOrEmpty(id) ? null : VisaoCameraDAO.Find(new AppFiltersCameraInput(id, allyId, otherWithFreeAccess))?.FirstOrDefault();
            if (camera == null)
                return new AppCameraDetailsOutput("Não foi encontrada nenhuma câmera!");

            var user = MobileAccountDAO.FindOne(x => x.Cellphone == mobileId);
            if (user == null)
                return new AppCameraDetailsOutput("Não foi encontrada nenhuma câmera!");

            return new AppCameraDetailsOutput(new AppCameraOutput(camera), VisaoFavoriteCamerasDAO.FindOne(x => x.CameraId == camera.Id && x.UserId == user.Id.ToString()) != null, GetCamerasList(new AppCameraListInput(new AppFiltersCameraInput(camera.Address.State, allyId, otherWithFreeAccess, new List<string> { camera.Address.City }), new PaginatorInput(1, 4))));
        }

        public AppCitiesOutput GetCities(string allyId, bool othersWithFreeAccess, AppFindCitiesTypeEnum type)
        {
            if (string.IsNullOrEmpty(allyId))
                return new("Id de Aliado não informado!");

            if (type == AppFindCitiesTypeEnum.Unknown)
                return new("FindCitiesTypeEnum não foi informado");

            var cities = type == AppFindCitiesTypeEnum.Featured ?
                new List<string> { "Caxias do Sul", "Farroupilha", "Nova Petrópolis", "Canela" } :
                VisaoCameraDAO.GetCities(new AppFiltersCameraInput(allyId, othersWithFreeAccess));

            if (!(cities?.Any() ?? false))
                return new("Nenhuma cidade cadastrada para este usuário");

            return new(cities.ToList());
        }
    }
}
