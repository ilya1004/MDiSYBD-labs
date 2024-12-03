namespace MusicPlayerDB.API.Extensions;

public static class AuthorizationPolicies
{
    public const string UserPolicy = "UserPolicy";
    public const string ArtistPolicy = "ArtistPolicy";
    public const string AdminPolicy = "AdminPolicy";
    public const string UserAndArtistPolicy = "UserAndArtistPolicy";
    public const string ArtistAndAdminPolicy = "ArtistAndAdminPolicy";
    public const string UserAndAdminPolicy = "UserAndAdminPolicy";
    public const string UserAndArtistAndAdminPolicy = "UserAndArtistAndAdminPolicy";
}
