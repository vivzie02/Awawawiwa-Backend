using System;
using System.Security.Cryptography;
using System.Text;
using static com.awawawiwa.Common.Constants.Constants;

namespace com.awawawiwa.Security
{
    /// <summary>
    /// PasswordHasherService
    /// </summary>
    public static class PasswordHasherService
    {
        /// <summary>
        /// ComputeHash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iteration"></param>
        /// <returns></returns>
        public static string ComputeHash(string password, string salt, int iteration)
        {
            if(iteration <= 0)
            {
                return password;
            }

            using var sha256 = SHA256.Create();
            var passwordSalt = $"{password}{salt}";
            var byteValue = Encoding.UTF8.GetBytes(passwordSalt);
            var byteHash = sha256.ComputeHash(byteValue);
            var hash = Convert.ToBase64String(byteHash);
            return ComputeHash(hash, salt, iteration - 1);
        }

        /// <summary>
        /// GenerateSalt
        /// </summary>
        /// <returns></returns>
        public static string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var byteSalt = new byte[16];
            rng.GetBytes(byteSalt);
            var salt = Convert.ToBase64String(byteSalt);
            return salt;
        }

        /// <summary>
        /// VerifyPassword
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        public static bool VerifyPassword(string password, string salt, string hashedPassword)
        {
            var computedHash = ComputeHash(password, salt, HASHING_ITERATIONS);
            return computedHash == hashedPassword;
        }
    }
}
