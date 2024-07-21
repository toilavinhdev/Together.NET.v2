namespace Together.Shared.Constants;

public static class TogetherRedisKeys
{
    public static string IdentityPrivilegeKey(object key) => $"identity-privilege:{key}";
    
    public static string ForgotPasswordTokenKey(object key) => $"forgot-passwd-token:{key}";
    
    public static string PostViewKey(object key) => $"post-view:{key}";

    public static string UserViewPostKey(object postKey, object userKey) => $"user-view-post:{postKey}:{userKey}";
}