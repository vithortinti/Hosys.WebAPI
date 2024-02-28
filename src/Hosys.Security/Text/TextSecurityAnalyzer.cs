using System.Text.RegularExpressions;
using Hosys.Application.Interfaces.Security.Text;

namespace Hosys.Security.Text
{
    public class TextSecurityAnalyzer : ITextSecurityAnalyzer
    {
        private readonly Regex _scriptTagRegex = new(@"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>");
        
        public bool HasScriptTag(string text)
        {
            return _scriptTagRegex.IsMatch(text);
        }
    }
}