using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace AvstickareBlazor
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //Läser token, ren sträng för att token ska kunna läsas
            var token = (await _localStorage.GetItemAsync<string>("authToken"))?.Trim('"');

            //validering av token
            if (string.IsNullOrWhiteSpace(token) || token.Count(c => c == '.') != 2)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            
            try
            {   
                //avkoda token
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                //skapa objekt med claims som representerar användaren
                var identity = new ClaimsIdentity(jwt.Claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                //returnera användare
                return new AuthenticationState(user);
            }
            catch
            {   
                //retuenra anonym användare
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        //meddelar att något har ändrats, exempelvis loggat in, då körs GetAuthenticationStateAsync
        public void NotifyUserChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}