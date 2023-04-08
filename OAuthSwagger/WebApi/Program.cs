using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace com.fabioscagliola.OAuthSwagger.WebApi;

public class Program
{
    static void Main(string[] args)
    {
        string[] scopes = { "offline_access", "openid", "profile", };

        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);

        IConfiguration azureAdB2CConfig = webApplicationBuilder.Configuration.GetSection(Constants.AzureAdB2C);

        webApplicationBuilder.Services.AddControllers();
        webApplicationBuilder.Services.AddEndpointsApiExplorer();
        webApplicationBuilder.Services.AddMicrosoftIdentityWebAppAuthentication(webApplicationBuilder.Configuration, Constants.AzureAdB2C);
        //webApplicationBuilder.Services.AddOptions();
        webApplicationBuilder.Services.Configure<OpenIdConnectOptions>(azureAdB2CConfig);

        webApplicationBuilder.Services.AddDbContext<WebApiDbContext>(optionsAction =>
        {
            optionsAction.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("ConnectionString"));
        });

        webApplicationBuilder.Services.AddSwaggerGen(setupAction =>
        {
            setupAction.EnableAnnotations();
            setupAction.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

            const string OAUTH2 = "oauth2";

            setupAction.AddSecurityDefinition(OAUTH2, new OpenApiSecurityScheme()
            {
                Flows = new()
                {
                    AuthorizationCode = new()
                    {
                        AuthorizationUrl = new($"{azureAdB2CConfig.GetValue<string>("Instance")}/{azureAdB2CConfig.GetValue<string>("Domain")}/{azureAdB2CConfig.GetValue<string>("SignUpSignInPolicyId")}/oauth2/v2.0/authorize"),
                        TokenUrl = new($"{azureAdB2CConfig.GetValue<string>("Instance")}/{azureAdB2CConfig.GetValue<string>("Domain")}/{azureAdB2CConfig.GetValue<string>("SignUpSignInPolicyId")}/oauth2/v2.0/token"),
                    },
                },
                Type = SecuritySchemeType.OAuth2,
            });

            setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new() { Reference = new OpenApiReference() { Id = OAUTH2, Type = ReferenceType.SecurityScheme, }, },
                        scopes
                    },
                });
        });

        WebApplication webApplication = webApplicationBuilder.Build();

        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI(setupAction =>
            {
                setupAction.OAuthClientId(azureAdB2CConfig.GetValue<string>("ClientId"));
                setupAction.OAuthClientSecret(azureAdB2CConfig.GetValue<string>("ClientSecret"));
                setupAction.OAuthScopes(scopes);
                setupAction.OAuthUsePkce();
            });
        }

        webApplication.UseAuthentication();
        webApplication.UseAuthorization();

        webApplication.UseHttpsRedirection();

        webApplication.MapControllers();

        webApplication.Run();
    }
}
