using System.Security.Cryptography;

namespace Common
{
    public class HashFunction
    {
        private const int saltSize = 16;
        private const int hashSize = 32;
        private const int iterations = 100000;
        private readonly HashAlgorithmName _algorithmName = HashAlgorithmName.SHA512;

        public (string,string) Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, _algorithmName, hashSize);
            return (Convert.ToHexString(hash), Convert.ToHexString(salt));
        }

        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] salt = Convert.FromHexString(storedSalt);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, _algorithmName, hashSize);
            var convertedHash = Convert.ToHexString(hash);
            return  convertedHash == storedHash;
        }
    }
}