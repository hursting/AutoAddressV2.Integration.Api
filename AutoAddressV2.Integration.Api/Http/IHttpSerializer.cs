namespace AutoAddressV2.Integration.Api.Http;

public interface IHttpSerializer
{
    string Serialize<T>(T value);
    ValueTask<T?> DeserializeAsync<T>(Stream stream);
}