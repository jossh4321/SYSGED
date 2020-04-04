﻿using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SISGED.Client.Auth
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //await Task.Delay(3000);
            var anonimo = new ClaimsIdentity(new List<Claim>()
            { new Claim("Llave", "valor1"),
               new Claim(ClaimTypes.Name,"Josue"),
               new Claim(ClaimTypes.Role,"admin")
            },"prueba");
            return await Task.FromResult(
                new AuthenticationState(
                    new ClaimsPrincipal(anonimo)));
        }
    }
}
