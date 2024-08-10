using System.ComponentModel;

namespace Together.Shared.Constants;

public enum TogetherRedisDatabase
{
    [Description("Default")]
    Default = 0,
    [Description("For user and role domains")]
    Identity = 1,
    [Description("For all forum domains")]
    Forum = 2,
    [Description("For chat domains")]
    Chat = 3
}

public class TogetherRedisKey
{
    public TogetherRedisDatabase Database { get; set; }

    public string KeyName { get; set; } = default!;
}

public static class TogetherRedisKeys
{
    public static TogetherRedisKey IdentityPrivilegeKey(object userId) => new()
    {
        Database = TogetherRedisDatabase.Identity,
        KeyName = $"identity-privilege:{userId}"
    };

    public static TogetherRedisKey ForgotPasswordTokenKey(object userId) => new()
    {
        Database = TogetherRedisDatabase.Identity,
        KeyName = $"forgot-passwd-token:{userId}"
    };
    
    public static TogetherRedisKey OnlineUserKeys() => new()
    {
        Database = TogetherRedisDatabase.Identity,
        KeyName = "online-users"
    };
    
    public static TogetherRedisKey PostViewKey(object postId) => new()
    {
        Database = TogetherRedisDatabase.Forum,
        KeyName = $"post-view:{postId}"
    }; 

    public static TogetherRedisKey PostViewerKey(object postId, object userId) => new()
    {
        Database = TogetherRedisDatabase.Forum,
        KeyName = $"post-viewer:{postId}:{userId}"
    };
}