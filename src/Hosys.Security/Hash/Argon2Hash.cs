using System.Text;
using Hosys.Application.Interfaces.Security.Hash;
using Konscious.Security.Cryptography;

namespace Hosys.Security.Hash
{
    public class Argon2Hash(string salt, string secret, string associatedData) : IHash
    {
        private readonly string _salt = salt;
        private readonly string _secret = secret;
        private readonly string _associatedData = associatedData;

        public async Task<string> HashAsync(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            var argon2 = CreateArgon2id(inputBytes);
            
            return Convert.ToBase64String(await argon2.GetBytesAsync(128));
        }

        public async Task<bool> VerifyAsync(string input, string hash)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = Encoding.UTF8.GetBytes(hash);
            
            var argon2 = CreateArgon2id(inputBytes);

            return (await argon2.GetBytesAsync(128)).SequenceEqual(hashBytes);
        }

        private Argon2id CreateArgon2id(byte[] inputBytes)
        {
            return new Argon2id(inputBytes)
            {
                DegreeOfParallelism = 16,
                MemorySize = 12583, // 12.000083923 MiB
                Iterations = 64,
                Salt = Encoding.UTF8.GetBytes(_salt),
                AssociatedData = Encoding.UTF8.GetBytes(_secret),
                KnownSecret = Encoding.UTF8.GetBytes(_associatedData)
            };
        }
    }
}