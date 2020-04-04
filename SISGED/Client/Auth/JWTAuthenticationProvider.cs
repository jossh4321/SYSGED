using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SISGED.Client.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace SISGED.Client.Auth
{
    public class JWTAuthenticationProvider : AuthenticationStateProvider, ILoginService
    {
        public JWTAuthenticationProvider(IJSRuntime js, HttpClient httpClient)
        {
            this.js = js;
            this.httpClient = httpClient;
        }
        public static readonly string TOKENKEY = "TOKENKEY";
        private readonly IJSRuntime js;
        private readonly HttpClient httpClient;

        private AuthenticationState Anonimo =>
            new AuthenticationState(new ClaimsPrincipal
                (new ClaimsIdentity()));
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //guardaremos el JWT en el LocalStorage
            //vificando si existe token en local storgae
            var token = await js.GetFromLocalStorage(TOKENKEY);
            if (string.IsNullOrEmpty(token))
            {
                return Anonimo;
            }
            return contruirAthenticationState(token);
        }
        private AuthenticationState contruirAthenticationState(string token)
        {
            //authentication in each request from the server
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", token);
            //extracting claims from headers
            return new AuthenticationState
                (new ClaimsPrincipal(new ClaimsIdentity
                (ParseClaimsFromJwt(token), "jwt")));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }
        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task Login(string token)
        {
            await js.SetInLocalStorage(TOKENKEY, token);
            var authState = contruirAthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await js.RemoveItem(TOKENKEY);
            //quitamos la cabecera por defecto
            httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
        }
    }
}
