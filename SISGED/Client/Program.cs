using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SISGED.Client.Repo;
using Microsoft.AspNetCore.Components.Authorization;
using SISGED.Client.Auth;

namespace SISGED.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            //setting DI and authorization/authentication configs
            ConfigureServices(builder.Services);
            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddScoped<IRepository, Repository>();
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
