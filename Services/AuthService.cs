using System.Net.Http.Json;
using AvstickareBlazor.Models;
using Blazored.LocalStorage;

//inloggning av användare
namespace AvstickareBlazor
{
    public class AuthService(HttpClient http, ILocalStorageService localStorage, CustomAuthStateProvider authProvider)
    {
        private readonly HttpClient _http = http;
        private readonly ILocalStorageService _localStorage = localStorage;
        private readonly CustomAuthStateProvider _authProvider = authProvider;

        public async Task<string?> LoginAsync(string email, string password)
        {
            //skickar e-post och lösenord till backend
            var response = await _http.PostAsJsonAsync("/api/Auth/logga-in", new { Email = email, Password = password });

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResult>();

            if (string.IsNullOrWhiteSpace(result?.Token))
            {
                return null;
            }

            //lagra i ls
            await _localStorage.SetItemAsync("authToken", result.Token);
            //uppdatera inloggningstillstånd
            _authProvider.NotifyUserChanged();
            return result.Token;
        }
    }
}