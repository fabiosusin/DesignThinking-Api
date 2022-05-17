using DAO.DBConnection;
using DAO.Intra.EquipamentDAO;
using DAO.Intra.Loan;
using DTO.General.Base.Api.Output;
using DTO.General.Image.Input;
using DTO.Intra.Equipament.Database;
using DTO.Intra.Equipament.Input;
using DTO.Intra.Equipament.Output;
using System.Collections.Generic;
using System.Linq;
using static Utils.Extensions.Files.Images.ImageFactory;

namespace Business.API.Hub.Equipament
{
    public class BlIntraEquipment
    {
        private readonly IntraLoanDAO IntraLoanDAO;
        private readonly IntraEquipmentDAO IntraEquipmentDAO;

        public BlIntraEquipment(XDataDatabaseSettings settings)
        {
            IntraLoanDAO = new(settings);
            IntraEquipmentDAO = new(settings);
        }

        public IntraEquipment GetById(string id) => IntraEquipmentDAO.FindById(id);

        public List<ImageFormat> GetEquipmentImages(string id, ListResolutionsSize size)
        {
            var equipment = GetById(id);
            if (equipment?.Images == null)
                return null;

            foreach (var image in equipment.Images)
                image.ImageUrl = image.GetImage(size, FileType.Jpeg);

            return equipment.Images;
        }

        public BaseApiOutput UpsertEquipment(IntraEquipment input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.Name))
                return new("Nome não informado!");

            if (string.IsNullOrEmpty(input.Code))
                return new("Código não informado!");

            if (string.IsNullOrEmpty(input.UserId))
                return new("UserId não informado!");

            if (IntraEquipmentDAO.FindOne(x => x.Code == input.Code && x.Id != input.Id) != null)
                return new("Já existe um Equipamento com este Código!");

            if (input.ImagesBase64?.Any() ?? false)
            {
                if (!(input.Images?.Any() ?? false))
                    input.Images = new List<ImageFormat>();

                foreach (var image in input.ImagesBase64)
                {
                    var imageResult = SaveListResolutions(image);
                    if (imageResult == null)
                        continue;

                    input.Images.Add(imageResult);
                }
            }

            var result = IntraEquipmentDAO.Upsert(input);
            if (result == null)
                return new("Não foi possível salvar o equipamento!");

            return new(true);
        }

        public BaseApiOutput DeleteEquipament(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new("Requisição mal formada!");

            var equipment = IntraEquipmentDAO.FindById(id);
            if (equipment == null)
                return new("Equipamento não encontrado!");

            if (equipment.Loaned)
                return new("Este equipamento está emprestado!");

            if (IntraLoanDAO.FindOne(x => x.EquipmentsIds.Contains(id)) != null)
                return new("O equipamento possui um empréstimo vinculado!");

            IntraEquipmentDAO.Remove(equipment);
            return new(true);
        }

        public IntraEquipmentListOutput List(IntraEquipmentListInput input)
        {
            var result = IntraEquipmentDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Equipamento encontrado!");

            return new(result);
        }
    }
}
