namespace KarateClub.HelperClasses
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            // Automatically generates salt and hashes
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // ===============================
        // VERIFY PASSWORD
        // ===============================
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(storedHash))
                return false;

            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    
    }
}
