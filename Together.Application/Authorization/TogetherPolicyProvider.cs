using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Together.Application.Authorization;

public sealed class TogetherPolicyProvider(IOptions<AuthorizationOptions> options) : IAuthorizationPolicyProvider
{
    private const string PolicyPrefix = "Together";
    
    private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; } = new(options);

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var pieces = policyName.Split(".");
        
        if (!policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase) || pieces.Length != 2)
            return await BackupPolicyProvider.GetPolicyAsync(policyName);
        
        var builder = new AuthorizationPolicyBuilder();
        builder.AddRequirements(new TogetherPolicyRequirement(pieces[1]));

        await Task.CompletedTask;
        return builder.Build();
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return BackupPolicyProvider.GetDefaultPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return BackupPolicyProvider.GetFallbackPolicyAsync();
    }
}