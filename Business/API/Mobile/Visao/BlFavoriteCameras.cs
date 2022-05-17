using DAO.DBConnection;
using DAO.Mobile.Account;
using DAO.Mobile.Visao;
using DTO.General.Base.Api.Output;
using DTO.General.Pagination.Input;
using DTO.Mobile.Visao.Database;
using DTO.Mobile.Visao.Input;
using DTO.Mobile.Visao.Output;
using System;
using System.Linq;

namespace Business.API.Mobile.Visao
{
    public class BlFavoriteCameras : BlBase
    {

        public BlFavoriteCameras(XDataDatabaseSettings settings) : base(settings) { }

        public AppCameraListOutput GetFavoriteCamerasList(AppFavoriteCameraListInput input)
        {
            if (string.IsNullOrEmpty(input?.Filters?.MobileId))
                return new("Nenhum MobileId informado");

            if (string.IsNullOrEmpty(input.Filters.AllyId))
                return new("Id de Aliado não informado");

            input.Filters.UserId = MobileAccountDAO.FindOne(x => x.AllyId == input.Filters.AllyId && x.Cellphone == input.Filters.MobileId)?.Id.ToString();
            if (string.IsNullOrEmpty(input.Filters.UserId))
                return new("Nenhum usuário encontrado");

            var paginator = input?.Paginator ?? new PaginatorInput(1, 4);

            if (paginator.Page <= 0)
                paginator.Page = 1;

            if (paginator.ResultsPerPage <= 0)
                paginator.ResultsPerPage = 4;

            var favoriteCameras = VisaoFavoriteCamerasDAO.Find(input)?.ToList();
            if (!(favoriteCameras?.Any() ?? false))
                return new("Usuário não possui nenhuma câmera favorita");

            var ids = favoriteCameras.Select(x => x.CameraId);
            var cameras = VisaoCameraDAO.Find(x => ids.Contains(x.Id)).ToList();
            if (!(cameras?.Any() ?? false))
                return new("Câmeras não encontradas no sistema");

            return new("Favoritas", "", cameras.Count, cameras.Select(x => new AppCameraOutput(x)).ToList());
        }

        public BaseApiOutput AddCameraBookmark(AppCameraBookmarkInput input)
        {
            var ids = CameraBookmarkIds(input);
            if (!ids.Success)
                return new(ids.Message);

            if (VisaoFavoriteCamerasDAO.FindOne(x => x.CameraId == ids.CameraId && x.UserId == ids.UserId) != null)
                return new("Câmera já está entre as favoritas");

            var result = VisaoFavoriteCamerasDAO.Insert(new AppVisaoUserFavoriteCamera { CameraId = ids.CameraId, UserId = ids.UserId });
            return result == null ? new("Não foi possível salvara câmera como favorita") : new BaseApiOutput(true);
        }

        public BaseApiOutput RemoveCameraBookmark(AppCameraBookmarkInput input)
        {
            var ids = CameraBookmarkIds(input);
            if (!ids.Success)
                return new(ids.Message);

            var camera = VisaoFavoriteCamerasDAO.FindOne(x => x.CameraId == ids.CameraId && x.UserId == ids.UserId);
            if (camera == null)
                return new("Câmera não está entre as favoritas");

            VisaoFavoriteCamerasDAO.Remove(camera);
            return new(true);
        }

        private AppCameraBookmarkIds CameraBookmarkIds(AppCameraBookmarkInput input)
        {
            if (input == null)
                return new("Requisição mal formada");

            if (string.IsNullOrEmpty(input.UserMobileId))
                return new("Nenhum MobileId informado");

            if (string.IsNullOrEmpty(input.AllyId))
                return new("Id de Aliado não informado");

            if (string.IsNullOrEmpty(input.CameraId))
                return new("Nenhuma câmera informada");

            var cameraId = VisaoCameraDAO.FindOne(x => x.Id == input.CameraId)?.Id.ToString();
            if (string.IsNullOrEmpty(cameraId))
                return new("Nenhuma câmera encontrada");

            var userId = MobileAccountDAO.FindOne(x => x.AllyId == input.AllyId && x.Cellphone == input.UserMobileId)?.Id.ToString();
            if (string.IsNullOrEmpty(userId))
                return new("Nenhum usuário encontrado");

            return new(cameraId, userId);
        }
    }
}
