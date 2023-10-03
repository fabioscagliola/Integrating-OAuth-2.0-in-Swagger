using com.fabioscagliola.OAuthSwagger.WebApi;
using NUnit.Framework;

namespace com.fabioscagliola.OAuthSwagger.WebApiTest;

public abstract class BaseTest
{
    protected WebApiTestWebApplicationFactory<Program> webApiTestWebApplicationFactory;

    [SetUp]
    public void Setup()
    {
        webApiTestWebApplicationFactory = new();
    }

    [TearDown]
    public void TearDown()
    {
        webApiTestWebApplicationFactory.Dispose();
    }
}
