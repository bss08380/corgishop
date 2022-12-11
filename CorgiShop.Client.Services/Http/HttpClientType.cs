using Ardalis.SmartEnum;

namespace CorgiShop.Client.Services.Http;

public class HttpClientType : SmartEnum<HttpClientType>
{
	public static HttpClientType SecureClient => new HttpClientType(nameof(SecureClient), 0, "CorgiShop.SecureClient");
    public static HttpClientType PublicClient => new HttpClientType(nameof(PublicClient), 0, "CorgiShop.PublicClient");

	public string FactoryName { get; }

    public HttpClientType(string name, int value, string factoryName)
		: base(name, value)
	{
        FactoryName = factoryName;

    }
}
