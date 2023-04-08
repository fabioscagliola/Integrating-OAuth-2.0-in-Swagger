using com.fabioscagliola.OAuthSwagger.WebApi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net.Http.Headers;

namespace com.fabioscagliola.OAuthSwagger.WebApiTest;

public abstract class BaseTest
{
    protected WebApiTestWebApplicationFactory<Program> webApiTestWebApplicationFactory;

    protected HttpClient httpClient;

    [SetUp]
    public void Setup()
    {
        webApiTestWebApplicationFactory = new();

        httpClient = webApiTestWebApplicationFactory.WithWebHostBuilder(config =>
        {
            config.ConfigureTestServices(servicesConfig =>
            {
                servicesConfig.AddAuthentication(WebApiTestAuthenticationHandler.AuthenticationScheme)
                    .AddScheme<AuthenticationSchemeOptions, WebApiTestAuthenticationHandler>(WebApiTestAuthenticationHandler.AuthenticationScheme, configureOptions => { });
            });
        }).CreateClient();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(WebApiTestAuthenticationHandler.AuthenticationScheme);
    }

    [TearDown]
    public void TearDown()
    {
        webApiTestWebApplicationFactory.Dispose();
    }
}
