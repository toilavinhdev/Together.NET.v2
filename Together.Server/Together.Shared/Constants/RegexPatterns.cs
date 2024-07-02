using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Together.Shared.Constants;

public static partial class RegexPatterns
{
    [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")] 
    private static partial Regex EmailGeneratedRegex();
    public static readonly Regex EmailRegex = EmailGeneratedRegex();
    
    
    [GeneratedRegex(@"^(?=.{6,24}$)[a-zA-Z0-9\._]*$")] 
    [Description("6-24 characters long; Allow a-z A-Z 0-9 and . /")]
    private static partial Regex UserNameGeneratedRegex();
    public static readonly Regex UserNameRegex = UserNameGeneratedRegex();
}