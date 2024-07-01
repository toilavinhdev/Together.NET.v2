using Microsoft.EntityFrameworkCore;
using Together.Domain.Aggregates.RoleAggregate;
using Together.Domain.Aggregates.UserAggregate;
using Together.Domain.Enums;
using Together.Shared.Constants;
using Together.Shared.Extensions;

namespace Together.Persistence;

public static class TogetherContextInitialization
{
    public static async Task SeedAsync(TogetherContext context)
    {
        await context.Database.MigrateAsync();

        await context.Database.EnsureCreatedAsync();
        
        var admin = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Administrator",
            UserName = "administrator",
            Email = "admin@together.net",
            PasswordHash = "admin".ToSha256(),
            Status = UserStatus.Active,
            Gender = Gender.Other
        };
        
        if (!await context.Users.AnyAsync(u => u.Email == admin.Email))
        {
            admin.MarkCreated();
            await context.Users.AddAsync(admin);
        }
        
        var defaultRoles = new List<Role>();
        foreach (var role in Roles)
        {
            var existed = await context.Roles.FirstOrDefaultAsync(r => r.Name == role.Name);
            if (existed is not null)
            {
                if (!existed.Claims!.SequenceEqual(role.Claims!))
                {
                    existed.Claims = role.Claims;
                }
                context.Roles.Update(existed);
            }
            else
            {
                role.UserRoles = role.Name == "Admin"
                    ? [ new UserRole { UserId = admin.Id }]
                    : null;
                role.MarkUserCreated(admin.Id);
                defaultRoles.Add(role);
            }
        }
        
        await context.Roles.AddRangeAsync(defaultRoles);
        
        await context.SaveChangesAsync();
    }
    
    private static readonly List<Role> Roles =
    [
        new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            IsDefault = true,
            Description = "For root administrators",
            Claims = [TogetherPolicies.All]
        },
        new Role
        {
            Id = Guid.NewGuid(),
            Name = "Member",
            IsDefault = true,
            Description = "For basic users",
            Claims = TogetherPolicies.RequiredPolicies().Distinct().ToList()
        }
    ];
}