using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HealthPetAPP.Client
{
    public class AutenticacionEstadoPersonalizada : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;

        public AutenticacionEstadoPersonalizada(ILocalStorageService localStorageService)
        {
            this._localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var estado = new AuthenticationState(new ClaimsPrincipal());
            string userName = await _localStorageService.GetItemAsStringAsync("userName");

            if (!string.IsNullOrEmpty(userName))
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }, "Autenticacion");

                estado = new AuthenticationState(new ClaimsPrincipal(identity));
            }

            NotifyAuthenticationStateChanged(Task.FromResult(estado));

            return estado;
        }
    }
}
