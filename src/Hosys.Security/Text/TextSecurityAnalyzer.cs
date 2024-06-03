using System.Text.RegularExpressions;
using Hosys.Security.Interfaces;

namespace Hosys.Security.Text
{
    public class TextSecurityAnalyzer : ITextSecurityAnalyzer
    {
        private readonly Regex _scriptTagRegex = new(@"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>");
        
        public bool HasScriptTag(string text)
        {
            return _scriptTagRegex.IsMatch(text);
        }

        public bool HasPathTraversal(string text)
        {
            return text.Contains("../");
        }
    }
}