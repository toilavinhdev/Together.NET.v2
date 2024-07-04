using Google.Apis.Auth;

namespace Together.Shared.Helpers;

public static class OAuthProvider
{
    public static async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleCredential(
        string clientId, 
        string credential)
    {
        try
        {
            return await GoogleJsonWebSignature.ValidateAsync(
                credential,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = [clientId]
                });
        }
        catch
        {
            return null;
        }
    }
}