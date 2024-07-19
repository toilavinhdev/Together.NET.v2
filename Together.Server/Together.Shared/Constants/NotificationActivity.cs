using System.Diagnostics.CodeAnalysis;

namespace Together.Shared.Constants;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class NotificationActivity
{
    public const string VOTE_POST = nameof(VOTE_POST);
    
    public const string REPLY_POST = nameof(REPLY_POST);
    
    public const string VOTE_REPLY = nameof(VOTE_REPLY);
    
    public const string REPLY_REPLY = nameof(REPLY_REPLY);
}