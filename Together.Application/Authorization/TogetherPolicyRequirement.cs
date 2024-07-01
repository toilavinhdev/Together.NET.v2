using Microsoft.AspNetCore.Authorization;

namespace Together.Application.Authorization;

public sealed class TogetherPolicyRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; set; } = permission;
}