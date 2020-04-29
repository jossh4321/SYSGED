using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SISGED.Client.Repo;
using SISGED.Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using SISGED.Client.Auth;
using SISGED.Shared.Validators;
using FluentValidation;
using Blazor.FileReader;

namespace SISGED.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            //setting DI and authorization/authentication configs
            ConfigureServices(builder.Services);
            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<ISwalFireMessage, SwalFireMessage>();
            services.AddFileReaderService(options => options.InitializeOnFirstCall = true);
            services.AddValidatorsFromAssemblyContaining<DatosValidator>();
            services.AddValidatorsFromAssemblyContaining<Usuario2Validator>();
            services.AddAuthorizationCore();
            services.AddScoped<JWTAuthenticationProvider> ();
            services.AddScoped<AuthenticationStateProvider, 
                JWTAuthenticationProvider>(
                provider => provider.GetRequiredService
                <JWTAuthenticationProvider>());
            services.AddScoped<ILoginService,
               JWTAuthenticationProvider>(
               provider => provider.GetRequiredService
               <JWTAuthenticationProvider>());
        }
    }
}
