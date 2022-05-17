using DTO.General.Base.Database;
using DTO.Hub.User.Input;

namespace DTO.Hub.User.Database
{
    public class HubUser : BaseData
    {
        public HubUser() { }
        public HubUser(HubAddUserInput input)
        {
            if (input == null)
                return;

            Name = input.Name;
            Id = input.UserId;
            Email = input.Email;
            Password = input.Password;
            Username = input.Username;
            AllyId = input.AllyId;
            IsMasterAdmin = input.IsMasterAdmin;
            TempPassword = input.TempPassword;
        }

        public bool IsMasterAdmin { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AllyId { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string TempPassword { get; set; }
    }

}
