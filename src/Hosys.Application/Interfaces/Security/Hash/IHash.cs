namespace Hosys.Application.Interfaces.Security.Hash
{
    public interface IHash
    {
        /// <summary>
        /// Hashes the input string using the specified algorithm.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> HashAsync(string input);

        /// <summary>
        /// Verifies the input string against the hash.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        Task<bool> VerifyAsync(string input, string hash);
    }
}