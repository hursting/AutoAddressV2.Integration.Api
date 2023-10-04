namespace AutoAddressV2.Integration.Api.Authentication;

public interface IProvideAuthentication
{
    Task<string> GetAuthenticationToken();
}