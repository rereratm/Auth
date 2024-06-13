using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using System.Text;

public static class SecurityKeyHelper
{
    public static SecurityKey CreateSecurityKey(string key)
    {
        // Derive a 64-byte key using SHA-256
        using (var sha256 = SHA256.Create())
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var hashBytes = sha256.ComputeHash(keyBytes);

            // Ensure the hash is at least 64 bytes
            var fullKeyBytes = new byte[64];
            Buffer.BlockCopy(hashBytes, 0, fullKeyBytes, 0, hashBytes.Length);
            return new SymmetricSecurityKey(fullKeyBytes);
        }
    }
}
