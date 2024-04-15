namespace TomatoGame.Service.Utils
{
    public static class EncryptAndDecrypter
    {
        public static string HashPassword(string password)
        {
            // Generate a salt and hash the password using bcrypt
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Verify the password against the hashed password using bcrypt
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
