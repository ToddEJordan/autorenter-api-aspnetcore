namespace AutoRenter.Api.Models
{
    public class UserModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsAdministrator { get; set; }

        public string BearerToken { get; set; }
    }
}
