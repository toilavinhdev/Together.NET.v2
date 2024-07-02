namespace Together.Shared.Constants;

public static class TogetherPolicies
{
    public static readonly string All = nameof(All);
    
    public static class User
    {
        public const string Me = "User:Me";
        public const string Get = "User:Get";
        public const string List = "User:List";
        public const string UpdateProfile = "User:UpdateProfile";
        public const string UpdatePassword = "User:UpdatePassword";
    }
    
    public static class Role
    {
        public const string List = "Role:List";
        public const string Create = "Role:Create";
        public const string Update = "Role:Update";
        public const string Assign = "Role:Assign";
        public const string Delete = "Role:Delete";
    }

    public static List<string> RequiredPolicies() => [
        User.Me,
        User.Get,
        User.UpdateProfile,
        User.UpdatePassword
    ];
}