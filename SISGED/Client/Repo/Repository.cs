using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SISGED.Client.Repo
{
    public class Repository :IRepository
    {
        public readonly HttpClient httpClient;
        private JsonSerializerOptions defaultJsonOptions =>
           new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        public Repository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var responseHttp = await httpClient.GetAsync(url);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserealizeResponse<T>(responseHttp, defaultJsonOptions);
                return new HttpResponseWrapper<T>(response, false, responseHttp);
            }
            else
            {
                return new HttpResponseWrapper<T>(default, true, responseHttp);
            }
        }
      
        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T send)
        {
            //serialize POCO object as a JSON Object
            var sendJSON = JsonSerializer.Serialize(send);
            //preparing body of Http Request
            var sendContent = new StringContent(sendJSON, Encoding.UTF8, "application/json");
            //response by url API endpoint and Request Body
            var responseHttp = await httpClient.PostAsync(url, sendContent);
            //getting de Response Object,errors and mesages by HttpClient
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T,TResponse>(string url, T send)
        {
            //serialize POCO object as a JSON Object
            var sendJSON = JsonSerializer.Serialize(send);
            //preparing body of Http Request
            var sendContent = new StringContent(sendJSON, Encoding.UTF8, "application/json");
            //response by url API endpoint and Request Body
            var responseHttp = await httpClient.PostAsync(url, sendContent);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserealizeResponse<TResponse>(responseHttp, defaultJsonOptions);
                return new HttpResponseWrapper<TResponse>(response, false, responseHttp);
            }
            else
            {
                return new HttpResponseWrapper<TResponse>(default, true, responseHttp);
            }
        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T requestBody)
        {
            var enviarJSON = JsonSerializer.Serialize(requestBody);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PutAsync(url, enviarContent);
            return new HttpResponseWrapper<object>(null,
                !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> Put<TResponse ,T>(string url, T send)
        {
            var sendJSON = JsonSerializer.Serialize(send);
            var sendContent = new StringContent(sendJSON, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PutAsync(url, sendContent);

            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserealizeResponse<TResponse>(responseHttp, defaultJsonOptions);
                return new HttpResponseWrapper<TResponse>(response, false, responseHttp);
            }
            else
            {
                return new HttpResponseWrapper<TResponse>(default, true, responseHttp);
            }
        }

        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            var responseHttp = await httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        private async Task<T> DeserealizeResponse<T>(HttpResponseMessage httpResponse,JsonSerializerOptions serializerJson)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>
                (responseString, serializerJson);
        }
        public List<OpcionDocumento> obtenerTiposDoc()
        {
            return  new List<OpcionDocumento>() {
                    new OpcionDocumento() { label = "Documento de Solicitud de Busqueda de Protocolo",value="SolicitudBPN" },
                    new OpcionDocumento() { label = "Documento de Oficio de Busqueda de Protocolo",value="OficioBPN" },
                    new OpcionDocumento() { label = "Documento de Resultado de Busqueda de Protocolo",value="ResultadoBPN" },
                    new OpcionDocumento() { label = "Documento de Solicitud de Expedicion de Firma",value="SolicitudExpedicionFirma" },
                    new OpcionDocumento() { label = "Documento de Oficio de Designacion de Notario",value="OficioDesignacionNotario" },
                    new OpcionDocumento() { label = "Documento de Conclusion de Firma",value="ConclusionFirma" },
                    new OpcionDocumento() { label = "Documento de Solicitud de Denuncia",value="SolicitudDenuncia" },
                    new OpcionDocumento() { label = "Documento de Aperturamiento Disciplinario",value="AperturamientoDisciplinario" },
                    new OpcionDocumento() { label = "Documento de Solicitud de Expediente de Notario",value="SolicitudExpedienteNotario" },
                    new OpcionDocumento() { label = "Documento de Dictamen",value="Dictamen" },
                    new OpcionDocumento() { label = "Documento de Resolucion",value="Resolucion" },
                    new OpcionDocumento() { label = "Documento de Apelacion",value="Apelacion" }};
        }

    }
}
