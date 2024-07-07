namespace Together.Shared.Constants;

public static class TogetherRedisKeys
{
    public static string GetIdentityPrivilegeKey(object userId) => $"identity-privilege:{userId}";
    
    public static string GetPostViewKey(object postId) => $"post-view:{postId}";

    public static string GetUserViewPostKey(object postId, object userId) => $"user-view-post:{postId}:{userId}";
}