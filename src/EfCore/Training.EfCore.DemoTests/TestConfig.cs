using Microsoft.Extensions.Configuration;

namespace Training.EfCore.DemoTests;

public static class TestConfig
{
	public static IConfiguration Config;

	static TestConfig()
	{
		Config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", true, true)
			.Build();
	}
}