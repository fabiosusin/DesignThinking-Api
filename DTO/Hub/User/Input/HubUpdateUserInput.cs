namespace DTO.Hub.User.Input
{
    public class HubUpdateUserInput
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string NewPassword { get; set; }
        public string PasswordValidation { get; set; }
    }
}
