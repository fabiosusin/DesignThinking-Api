using Business.API.Mobile.Files;
using DAO.DBConnection;
using DAO.External.Visao;
using DAO.General.Log;
using DTO.External.Visao.Database;
using DTO.External.Visao.Enum;
using DTO.External.Visao.Input;
using DTO.External.Visao.Output;
using DTO.General.Base.Api.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using static Utils.Extensions.Files.Images.ImageFactory;

namespace Business.API.External.Visao
{
    public class BlCameras
    {
        protected VisaoCameraDAO VisaoCameraDAO;
        protected LogHistoryDAO LogHistoryDAO;
        protected BlImage BlImage;
        public BlCameras(XDataDatabaseSettings settings)
        {
            VisaoCameraDAO = new(settings);
            LogHistoryDAO = new(settings);
            BlImage = new();
        }

        public CameraExternalOutput GetCamera(CameraListExternalInput input)
        {
            var result = GetCamerasList(input);
            return !result.Success ? new(result.Message) : new(result.Cameras.FirstOrDefault());
        }

        public CameraListExternalOutput GetCamerasList(CameraListExternalInput input) => string.IsNullOrEmpty(input?.Filters?.AllyId) ? new("Informe o Id de Aliado da Câmera!") : new(VisaoCameraDAO.Find(input).ToList());

        public List<BaseApiOutput> RegisterCameras(List<VisaoCameraInput> input)
        {
            var results = new List<BaseApiOutput>();
            foreach (var item in input)
            {
                var result = RegisterCamera(item);
                if (!result.Success)
                    result.Message += " Câmera: " + item.Name;

                results.Add(result);
            }

            return results;
        }

        public BaseApiOutput RegisterCamera(VisaoCameraInput input)
        {
            var baseValidation = BaseValidation(input);
            if (!(baseValidation?.Success ?? false))
                return baseValidation;

            if (VisaoCameraDAO.FindOne(x => x.CameraLink == input.CameraLink) != null)
                return new("Câmera já cadastrada com este Link!");

            var camera = new VisaoCamera(input)
            {
                CameraImg = GenerateCameraImage(input.CameraImgLink, input.CameraImgBase64)
            };

            if (string.IsNullOrEmpty(camera.CameraImg))
                return new("Não foi possível salvar a imagem para a câmera!");

            foreach (var partner in input.Partners)
            {
                var logo = GeneratePartnerImage(partner.LogoLink, partner.LogoBase64, 200);
                var logoInline = GeneratePartnerImage(partner.LogoInlineLink, partner.LogoInlineBase64, 500);
                var newPartner = new PartnerData(partner.Name, logo, logoInline, partner.Description, partner.Support, partner.Type);

                if (string.IsNullOrEmpty(newPartner.Logo))
                    return new("Não foi possível salvar a imagem para o parceiro: " + newPartner.Name);

                camera.Partners.Add(newPartner);
            }

            VisaoCameraDAO.Insert(camera);
            return new(true);
        }

        public BaseApiOutput UpdateCamera(VisaoCameraInput input)
        {
            var baseValidation = BaseValidation(input);
            if (!(baseValidation?.Success ?? false))
                return baseValidation;

            var existing = VisaoCameraDAO.FindOne(x => x.CameraLink == input.CameraLink);
            if (existing == null)
                return new("Nenhuma câmera cadastrada com este Link!");

            // Se alterou a imagem, deleta a antiga e após isso adiciona a nova
            if (!string.IsNullOrEmpty(input.CameraImgBase64) || existing.CameraImg != input.CameraImgLink)
                _ = !BlImage.RemoveImage(existing.CameraImg);

            existing.CameraImg = GenerateCameraImage(input.CameraImgLink, input.CameraImgBase64);
            var partners = new List<PartnerData>();
            var partnersImgLinks = new List<string>();
            foreach (var partner in input.Partners)
            {
                var logo = GeneratePartnerImage(partner.LogoLink, partner.LogoBase64, 200);
                var logoInline = GeneratePartnerImage(partner.LogoInlineLink, partner.LogoInlineBase64, 500);
                var newPartner = new PartnerData(partner.Name, logo, logoInline, partner.Description, partner.Support, partner.Type);
                if (string.IsNullOrEmpty(newPartner.Logo))
                    return new("Não foi possível salvar a imagem para o parceiro: " + newPartner.Name);

                partnersImgLinks.Add(newPartner.Logo);
                partners.Add(newPartner);
            }

            // Como não é possivel fazer nenhuma associação, dos novos parceiros com os existentes, apaga as imagens dos parceiros antigos,
            // caso o link dos logos abaixo estejam em "partnersImgLinks", ele não remove a imagem, por estar associada a um parceiro
            existing.Partners.Where(x => !partnersImgLinks.Contains(x.Logo)).ToList().ForEach(partner => _ = !BlImage.RemoveImage(partner.Logo));

            VisaoCameraDAO.Update(new VisaoCamera(existing.Id, existing.CameraImg, existing.Partners, input));
            return new(true);
        }

        public BaseApiOutput DeleteCamera(string cameraLink)
        {
            if (string.IsNullOrEmpty(cameraLink))
                return new("Requisição mal formada!");

            var existing = VisaoCameraDAO.FindOne(x => x.CameraLink == cameraLink);
            if (existing == null)
                return new("Nenhuma câmera cadastrada com este Link!");

            VisaoCameraDAO.Remove(existing);
            return new(true);
        }

        private static string GenerateCameraImage(string link, string base64) => string.IsNullOrEmpty(link) && string.IsNullOrEmpty(base64) ?
            null : link ?? SaveImageFromBase64(base64, 500, GetImagesEnum.Jpeg)?.ImageJpeg;

        private static string GeneratePartnerImage(string link, string base64, int size) => string.IsNullOrEmpty(link) && string.IsNullOrEmpty(base64) ?
            null : link ?? SaveImageFromBase64(base64, size, GetImagesEnum.Png)?.ImagePng;

        private static BaseApiOutput BaseValidation(VisaoCameraInput input)
        {

            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.AllyId))
                return new("Id de Aliado não informado!");

            if (string.IsNullOrEmpty(input.CameraLink))
                return new("Nenhum link de câmera informada!");

            if (string.IsNullOrEmpty(input.Name))
                return new("Nenhum nome de câmera informado!");

            var hasCamImage = new List<bool> { !string.IsNullOrEmpty(input.CameraImgBase64), !string.IsNullOrEmpty(input.CameraImgLink) };
            if (!hasCamImage.Any(x => x))
                return new("Nenhuma imagem de câmera informada!");

            if (hasCamImage.Where(x => x).Count() > 1)
                return new("Informe apenas um link ou base64 de imagem de câmera!");

            if (input.CameraImgLink?.Length > 1000)
                return new("Link de imagem de câmera, excede o limite de caracteres!");

            if (!(input.Partners?.Any() ?? false))
                return new("Nenhum parceiro informado!");

            foreach (var partner in input.Partners)
            {
                if (partner.Type == VisaoCameraPartnerType.Unknown)
                    return new("Informe o tipo para todos parceiros!");

                var hasPartnerCamImage = new List<bool> { !string.IsNullOrEmpty(partner.LogoLink), !string.IsNullOrEmpty(partner.LogoBase64) };
                if (!hasPartnerCamImage.Any(x => x))
                    return new("Informe uma imagem para todos parceiros!");

                if (hasPartnerCamImage.Where(x => x).Count() > 1)
                    return new("Informe apenas um link ou base64 de imagem para todos parceiros!");

                if (partner.LogoLink?.Length > 1000)
                    return new("Link de imagem de câmera, excede o limite de caracteres!");

                var hasPartnerCamInlineImage = new List<bool> { !string.IsNullOrEmpty(partner.LogoInlineLink), !string.IsNullOrEmpty(partner.LogoInlineBase64) };
                if (hasPartnerCamInlineImage.Where(x => x).Count() > 1)
                    return new("Informe apenas um link ou base64 de imagem para todos parceiros!");

                if (partner.LogoInlineLink?.Length > 1000)
                    return new("Link de imagem de câmera, excede o limite de caracteres!");
            }

            return new(true);
        }
    }
}
