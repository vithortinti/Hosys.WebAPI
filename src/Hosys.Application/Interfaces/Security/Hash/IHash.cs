namespace Hosys.Application.Interfaces.Security.Hash
{
    public interface IHash
    {
        Task<string> HashAsync(string input);
        Task<bool> VerifyAsync(string input, string hash);
    }
}