namespace TomatoGame.Web.Models
{
    public class UserSignInSignUpViewModal
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Name { get; set; }

        //login
        public string LoginEmail { get; set; }
        public string LoginPassword { get; set; }
    }
}