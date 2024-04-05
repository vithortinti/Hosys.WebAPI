namespace Hosys.Security.Interfaces
{
    public interface ITextSecurityAnalyzer
    {
        bool HasScriptTag(string text);
        bool HasPathTraversal(string text);
    }
}