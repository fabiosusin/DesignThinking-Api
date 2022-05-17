using DAO.DBConnection;
using DAO.Hub.Application.Database;
using DTO.General.Base.Api.Output;
using DTO.General.Image.Input;
using DTO.Hub.Application.Sponsor.Database;
using DTO.Hub.Application.Sponsor.Input;
using static Utils.Extensions.Files.Images.ImageFactory;

namespace Business.API.Hub.Application.Sponsor
{
    public class BlHubAppSponsor
    {
        protected AppSponsorDAO AppSponsorDAO;
        public BlHubAppSponsor(XDataDatabaseSettings settings) => AppSponsorDAO = new(settings);

        public BaseApiOutput UpsertAppSponsor(HubAppSponsorInput input)
        {
            if (input == null)
                return new("Requisição mal formada");

            if (string.IsNullOrEmpty(input.AllyId))
                return new("Id de Aliado não informado!");

            AppSponsor existing = null;
            if (string.IsNullOrEmpty(input.Id))
            {
                if (AppSponsorDAO.FindOne(x => x.Title == input.Title) != null)
                    return new("Patrocinador já cadastrado com o mesmo nome para este aliado!");
            }
            else
            {
                existing = AppSponsorDAO.FindById(input.Id);
                if (string.IsNullOrEmpty(existing.Id))
                    return new("Patrocinador não encontrado!");

                if (existing.AllyId != input.AllyId)
                    return new("Patrocinador não pertence a este aliado!");
            }

            ImageFormat img = null;
            if (!input.SaveImg)
                img = existing?.Image;
            else
            {
                img = SaveListResolutions(input.ImageBase64);
                if (img == null)
                    return new("Não foi possível salvar o Logo");
            }

            if (string.IsNullOrEmpty(existing?.Id))
                AppSponsorDAO.Insert(new(input, img));
            else
                AppSponsorDAO.Update(new(existing.Id, input, img));

            return new(true);
        }
    }
}
