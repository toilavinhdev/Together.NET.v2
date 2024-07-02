namespace Together.Shared.Constants;

public static class TogetherRedisKeys
{
    public static string GetIdentityPrivilegeKey(object userId) => $"identity-privilege:{userId}";
}