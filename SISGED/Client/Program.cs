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
using SISGED.Shared.Models;
using SISGED.Shared.Validators.DocumentosValidator.DesignacionNotario;
using SISGED.Shared.Validators.DocumentosValidator.OficioBPN;
using SISGED.Shared.Validators.DocumentosValidator.SolicitudDenuncia;
using SISGED.Shared.Validators.DocumentosValidator.SolicitudPBN;
using SISGED.Shared.Entities;
using SISGED.Shared.Validators.DocumentosValidator.AperturamientoDisciplinario;
using SISGED.Shared.Validators.FiltroEscrituraPublicaValidator;
using SISGED.Shared.Validators.UsuarioValidator;
using SISGED.Shared.Validators.EstadisticasValidator;
using SISGED.Shared.Validators.DocumentosValidator.SolicitudInicial;
using SISGED.Shared.Validators.DocumentosValidator.Apelacion;
using SISGED.Shared.Validators.DocumentosValidator.Dictamen;
using SISGED.Shared.Validators.DocumentosValidator.EntregaExpedienteNotario;
using SISGED.Shared.Validators.DocumentosValidator.ResultadoBPN;
using SISGED.Shared.Validators.DocumentosValidator.ConclusionFirma;
using SISGED.Shared.Validators.DocumentosValidator.Resolucion;
using SISGED.Shared.Validators.PasosValidator;

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
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<ISwalFireMessage, SwalFireMessage>();
            services.AddFileReaderService(options => options.InitializeOnFirstCall = true);
            
            //Datos del usuario
            services.AddValidatorsFromAssemblyContaining<DatosValidator>();
            services.AddValidatorsFromAssemblyContaining<Usuario2Validator>();
            services.AddValidatorsFromAssemblyContaining<UsuarioValidator>();

            // Solicitud Inicial DTO
            services.AddValidatorsFromAssemblyContaining<ContenidoSolicitudInicialDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<SolicitudInicialDTOValidator>();

            // Solicitud de búsqueda de protocolo notarial
            services.AddValidatorsFromAssemblyContaining<ContenidoSolicitudBPNDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<SolicitudBPNDTOValidator>();

            // Oficio de búsqueda de protocolo notarial
            services.AddValidatorsFromAssemblyContaining<ContenidoOficioBPNValidator>();
            services.AddValidatorsFromAssemblyContaining<OficioBPNValidator>();

            //Resultado de búsqueda de protocolo notarial
            services.AddValidatorsFromAssemblyContaining<ContenidoResultadoBPNValidator>();
            services.AddValidatorsFromAssemblyContaining<ResultadoBPNValidator>();

            //Solicitud de expedición de firmas
            services.AddValidatorsFromAssemblyContaining<ContenidoSEFValidator>();
            services.AddValidatorsFromAssemblyContaining<SolicitudExpedicionFirmaValidator>();

            //Designación de notario
            services.AddValidatorsFromAssemblyContaining<ContenidoODNValidator>();
            services.AddValidatorsFromAssemblyContaining<OficioDesignacionNotarioValidator>();

            //Conclusión de la firma
            services.AddValidatorsFromAssemblyContaining<ContenidoCFValidator>();
            services.AddValidatorsFromAssemblyContaining<CFValidator>();

            // Solicitud de denuncia
            services.AddValidatorsFromAssemblyContaining<ContenidoSolicitudDenunciaDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<SolicitudDenunciaDTOValidator>();

            //Apelación
            services.AddValidatorsFromAssemblyContaining<ContenidoADValidator>();
            services.AddValidatorsFromAssemblyContaining<ADValidator>();
            services.AddValidatorsFromAssemblyContaining<AperturamientoDisciplinarioValidator>();

            //Solicitud de expedientes de notario
            services.AddValidatorsFromAssemblyContaining<ContenidoSolicitudExpedienteNotario>();
            services.AddValidatorsFromAssemblyContaining<SolicitudExpedicionFirmaValidator>();

            //Entrega expediente del notario
            services.AddValidatorsFromAssemblyContaining<ContenidoEntregaExpedienteNotarioValidator>();
            services.AddValidatorsFromAssemblyContaining<EntregaExpedienteNotarioValidator>();

            //Dictamen
            services.AddValidatorsFromAssemblyContaining<ContenidoDictamenValidator>();
            services.AddValidatorsFromAssemblyContaining<DictamenValidator>();

            //Resolución
            services.AddValidatorsFromAssemblyContaining<ContenidoResolucionValidator>();
            services.AddValidatorsFromAssemblyContaining<ResolucionValidator>();

            // Apelación
            services.AddValidatorsFromAssemblyContaining<ContenidoApelacionValidator>();
            services.AddValidatorsFromAssemblyContaining<ApelacionValidator>();



            //services.AddValidatorsFromAssemblyContaining<ParticipanteValidator>();
            //services.AddValidatorsFromAssemblyContaining<HechoValidator>();
            
            //Parámetros de búsqueda de escritura pública
            services.AddValidatorsFromAssemblyContaining<ParametrosBusquedaEscrituraPublicaValidator>();

            
            services.AddValidatorsFromAssemblyContaining<EstadisticaDocXMesDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<EstadisticaDocXAreaXMesValidator>();
            services.AddValidatorsFromAssemblyContaining<EstadisticaDocCaducadosValidator>();


            services.AddValidatorsFromAssemblyContaining<InformacionSubpaso>();
            services.AddValidatorsFromAssemblyContaining<SubPasosValidator>();
            services.AddValidatorsFromAssemblyContaining<DocumentosPasosValidator>();
            services.AddValidatorsFromAssemblyContaining<PasosValidator>();

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
            services.AddScoped<Sesion>();
        }
    }
}
