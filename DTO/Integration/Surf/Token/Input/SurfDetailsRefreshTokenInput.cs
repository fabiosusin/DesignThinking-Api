namespace DTO.Integration.Surf.Token.Input
{
    public class SurfDetailsRefreshTokenInput
    {
        public string Login { get; set; }
        public string RefreshToken { get; set; }
        public string GrantType { get; set; }
    }
}
