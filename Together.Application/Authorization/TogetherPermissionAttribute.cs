using Microsoft.AspNetCore.Authorization;

namespace Together.Application.Authorization;

public sealed class TogetherPermissionAttribute : AuthorizeAttribute
{
    private const string PolicyPrefix = "Together";

    public string Permission { get; }

    public TogetherPermissionAttribute(string permission)
    {
        Permission = permission;
        SetPolicy();
    }

    private void SetPolicy()
    {
        Policy = $"{PolicyPrefix}.{Permission}";
    }
}