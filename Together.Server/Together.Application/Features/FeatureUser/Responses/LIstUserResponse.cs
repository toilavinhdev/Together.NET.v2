namespace Together.Application.Features.FeatureUser.Responses;

public class ListUserResponse : PaginationResult<UserViewModel>
{
    
}

public class UserViewModel : BaseEntity
{
    public string UserName { get; set; } = default!;

    public string? Avatar { get; set; }
}