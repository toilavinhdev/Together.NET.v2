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
    
    public static class Forum
    {
        public const string View = "Forum:View";
        public const string Create = "Forum:Create";
    }
    
    public static class Topic
    {
        public const string Create = "Topic:Create";
    }
    
    public static class Prefix
    {
        public const string View = "Prefix:View";
        public const string Create = "Prefix:Create";
    }
    
    public static class Post
    {
        public const string View = "Post:View";
        public const string Create = "Post:Create";
    }
    
    public static class Reply
    {
        public const string Create = "Reply:Create";
    }

    public static List<string> RequiredPolicies() => [
        User.Me,
        User.Get,
        User.UpdateProfile,
        User.UpdatePassword
    ];
}