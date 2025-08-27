namespace WebAPI_Demo.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }

        public string Roles { get; set; }

        public string RefreshToken { get; set; }
    }
}
