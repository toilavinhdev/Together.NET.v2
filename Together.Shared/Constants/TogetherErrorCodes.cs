namespace Together.Shared.Constants;

public static class TogetherErrorCodes
{
    public static class Server
    {
        public const string BadRequest = nameof(BadRequest);
        public const string Unauthorized = nameof(Unauthorized);
        public const string Forbidden = nameof(Forbidden);
        public const string NotFound = nameof(NotFound);
        public const string UnsupportedMediaType = nameof(UnsupportedMediaType);
        public const string InternalServer = nameof(InternalServer);
    }
    
    public static class User
    {
        public const string AccountHasBeenBanned = nameof(AccountHasBeenBanned);
        public const string UserNotFound = nameof(UserNotFound);
        public const string UserNameAlreadyExists = nameof(UserNameAlreadyExists);
        public const string UserEmailAlreadyExists = nameof(UserEmailAlreadyExists);
        public const string IncorrectPassword = nameof(IncorrectPassword);
    }
    
    public static class Role
    {
        public const string RoleNotFound = nameof(RoleNotFound);
    }
}