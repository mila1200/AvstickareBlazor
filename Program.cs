using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AvstickareBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5013/") });

//service
builder.Services.AddScoped<TripService>();
builder.Services.AddScoped<PlaceService>();
builder.Services.AddScoped<FavoriteService>();

//inkludera mubblazor
builder.Services.AddMudServices();

await builder.Build().RunAsync();
