namespace Hosys.Application.Interfaces.Security.Text
{
    public interface ITextSecurityAnalyzer
    {
        bool HasScriptTag(string text);
    }
}