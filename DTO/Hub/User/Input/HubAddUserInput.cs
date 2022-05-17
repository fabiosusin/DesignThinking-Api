using DTO.Hub.User.Database;
using static DTO.Hub.User.Enums.HubRouteRype;

namespace DTO.Hub.User.Input
{
    public class HubAddUserInput
    {
        public bool IsMasterAdmin { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordValidation { get; set; }
        public string Username { get; set; }
        public string AllyId { get; set; }
        public string TempPassword { get; set; }
        public HubRouteType Permissions { get; set; }
    }
}
