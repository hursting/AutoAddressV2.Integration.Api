using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoAddressV2.Integration.Api.Http;

public class SimpleHttpSerializer : IHttpSerializer
{
    
    private readonly JsonSerializerOptions _options;

    public SimpleHttpSerializer(JsonSerializerOptions? options = null)
    {
        _options = options ?? new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Converters = {new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)}
        };
    }

    public string Serialize<T>(T value) => JsonSerializer.Serialize(value, _options);

    public ValueTask<T?> DeserializeAsync<T>(Stream stream) => JsonSerializer.DeserializeAsync<T>(stream, _options);
   
}