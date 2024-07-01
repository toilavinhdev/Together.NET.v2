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
}