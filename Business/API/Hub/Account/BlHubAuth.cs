using DAO.DBConnection;
using DAO.Hub.AllyDAO;
using DAO.Hub.Order;
using DAO.Hub.Permission;
using DAO.Hub.UserDAO;
using DTO.General.Base.Api.Output;
using DTO.General.Email.Input;
using DTO.General.Login.Input;
using DTO.Hub.Permission.Database;
using DTO.Hub.User.Database;
using DTO.Hub.User.Input;
using DTO.Hub.User.Output;
using Services.Mobile.Email;
using System.Linq;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Business.API.Hub.Account
{
    public class BlHubAuth
    {
        private readonly HubUserDAO UserDAO;
        private readonly HubAllyDAO AllyDAO;
        private readonly HubOrderDAO HubOrderDAO;
        private readonly HubUserPermissionDAO HubUserPermissionDAO;

        public BlHubAuth(XDataDatabaseSettings settings)
        {
            HubOrderDAO = new(settings);
            UserDAO = new(settings);
            AllyDAO = new(settings);
            HubUserPermissionDAO = new(settings);
        }

        public HubLoginOutput FindAccount(LoginInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.Email))
                return new("Email não informado!");

            if (string.IsNullOrEmpty(input.Password))
                return new("Senha não informada!");

            var account = UserDAO.FindOne(x => x.Email == input.Email && x.Password == input.Password);
            if (account == null)
                account = UserDAO.FindOne(x => x.Email == input.Email && x.TempPassword == input.TempPassword);

            if (!string.IsNullOrEmpty(account?.TempPassword) && account.Password == input.Password)
            {
                account.TempPassword = null;
                _ = UserDAO.Update(account);
            }

            var isMasterAlly = AllyDAO.FindById(account?.AllyId)?.IsMasterAlly ?? false;
            return account == null ? new("Não foi possível encontrar o usuário!") : new(account, isMasterAlly);
        }

        public HubLoginOutput FindAccountByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new("Email não informado!");

            if (!email.IsValidEmail())
                return new("Email inválido informado!");

            var account = UserDAO.FindOne(x => x.Email == email);
            var isMasterAlly = AllyDAO.FindById(account?.AllyId)?.IsMasterAlly ?? false;
            return account == null ? new("Não foi possível encontrar o usuário!") : new(account, isMasterAlly);
        }

        public BaseApiOutput UpsertUser(HubAddUserInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.Name))
                return new("Nome não informado!");

            if (string.IsNullOrEmpty(input.Email))
                return new("Email não informado!");

            if (string.IsNullOrEmpty(input.AllyId))
                return new("Id de Aliado não informado!");

            if (string.IsNullOrEmpty(input.Username))
                return new("Nome de Usuário não informado!");

            if (string.IsNullOrEmpty(input.UserId))
            {
                if (string.IsNullOrEmpty(input.Password))
                    return new("Senha não informada!");

                if (FindAccountByEmail(input.Email).Success)
                    return new("Usuário já cadastrado com este Email!");

                if (UserDAO.FindOne(x => x.Username == input.Username) != null)
                    return new("Usuário já cadastrado com este nome!");

            }
            
            if (!string.IsNullOrEmpty(input.Password) && input.Password != input.PasswordValidation)
                return new("As senhas não coincidem!");

            var permissions = input.Permissions;

            if (input.IsMasterAdmin)
            {
                var masterUser = UserDAO.FindOne(x => x.IsMasterAdmin == true);
                if (!string.IsNullOrEmpty(masterUser?.Id) && masterUser.Id != input.UserId)
                    return new("Usuário admin já cadastrado!");
            }

            if (AllyDAO.FindById(input.AllyId) == null)
                return new("Aliado não encontrado!");
     
            var result = string.IsNullOrEmpty(input.UserId) ? UserDAO.Insert(new HubUser(input)) : UserDAO.Update(new HubUser(input));
            if(result == null)
                return new("Não foi possível salvar o usuário!");

            var existingPermission = HubUserPermissionDAO.FindOne(x => x.UserId == input.UserId && x.AllyId == input.AllyId);
            return existingPermission == null ? HubUserPermissionDAO.Insert(new HubUserPermission(input.AllyId, result?.Data?.Id, permissions)) : HubUserPermissionDAO.Update(new HubUserPermission(input.AllyId, input.UserId, existingPermission.Id, permissions));
        }

        public BaseApiOutput UpdateUser(HubUpdateUserInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.UserId))
                return new("Id de usuário não informado!");

            var account = UserDAO.FindOne(x => x.Id == input.UserId);
            if (account == null)
                return new("Usuário não encontrado!");

            if (!string.IsNullOrEmpty(input.NewPassword))
            {
                if (string.IsNullOrEmpty(input.PasswordValidation))
                    return new("Senha de verificação não informada!");

                if (input.NewPassword != input.PasswordValidation)
                    return new("As senhas não coincidem!");

                account.Password = input.NewPassword;
            }

            if (string.IsNullOrEmpty(input.Name))
                return new("Nome não informado!");

            account.TempPassword = null;
            account.Name = input.Name;
            var result = UserDAO.Update(account);
            return result == null ? new("Não foi possível salvar as alterações do usuário!") : new(true);
        }

        public async Task<BaseApiOutput> SendTempPassword(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
                return new("Email de usuário não informado!");

            var account = UserDAO.FindOne(x => x.Email == userEmail);
            if (account == null)
                return new("Usuário não encontrado!");

            account.TempPassword = StringExtension.RandomString(6, true);
            var msg = "Sua senha temporária de acesso ao App é: " + account.TempPassword;
            var sendEmail = await new EmailService().SendEmail(new EmailRequestInput(account.Email, "Senha Temporária", msg)).ConfigureAwait(false);
            if (sendEmail)
            {
                _ = UserDAO.Update(account);
                return new BaseApiOutput(true, "Senha temporária enviada no Email: " + account.Email);
            }

            return new BaseApiOutput(false, "Não foi possível enviar a senha temporária para o Email: " + account.Email);
        }

        public BaseApiOutput DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new("Requisição mal formada!");

            var user = UserDAO.FindById(id);
            if (user == null)
                return new("Usuário não encontrado!");

            if (HubOrderDAO.FindOne(x => x.SellerId == id) != null)
                return new("Usuário possui vendas vinculadas!");

            HubUserPermissionDAO.Remove(HubUserPermissionDAO.FindOne(x => x.UserId == id));

            UserDAO.Remove(user);
            return new(true);
        }

        public HubUserListOutput List(HubUserListInput input)
        {
            var result = UserDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Usuário encontrado!");

            return new(result);
        }
    }
}
