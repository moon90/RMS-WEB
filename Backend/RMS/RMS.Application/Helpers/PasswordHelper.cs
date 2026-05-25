using System;
using System.Security.Cryptography;
using System.Text;

namespace RMS.Application.Helpers
{
    public static class PasswordHelper
    {
        public static (string Hash, string Salt) CreatePasswordHash(string password)
        {
            // Generate a 128-bit (16-byte) key for the salt
            byte[] saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);

            // Encode salt in Base64Url
            string salt = Base64UrlEncode(saltBytes);

            // Compute the hash using HMACSHA256
            using var hmac = new HMACSHA256(saltBytes);
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Encode full hash in Base64Url
            string hash = Base64UrlEncode(hashBytes);

            return (hash, salt);
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            // Decode salt from Base64Url
            byte[] saltBytes = Base64UrlDecode(storedSalt);

            // Compute the hash using HMACSHA256
            using var hmac = new HMACSHA256(saltBytes);
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Backward compatibility: If the stored hash is 128-bit (truncated to 16 bytes)
            // 16 bytes in Base64Url is typically 22 characters.
            if (storedHash.Length == 22)
            {
                byte[] truncatedHash = new byte[16];
                Array.Copy(hashBytes, truncatedHash, 16);
                string computedTruncatedHash = Base64UrlEncode(truncatedHash);
                if (computedTruncatedHash == storedHash) return true;
            }

            // Encode full hash in Base64Url
            string computedHash = Base64UrlEncode(hashBytes);

            // Compare the computed hash with the stored hash
            return computedHash == storedHash;
        }

        // Helper method to encode bytes in Base64Url format
        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }

        // Helper method to decode Base64Url to bytes
        private static byte[] Base64UrlDecode(string input)
        {
            string base64 = input
                .Replace('-', '+')
                .Replace('_', '/');

            // Pad the base64 string with '=' to make its length a multiple of 4
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }

    }
}
