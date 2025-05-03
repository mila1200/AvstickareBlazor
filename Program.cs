using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AvstickareBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//service, singleton för att datan rutten ska vara kvar när användaren går från kartan
builder.Services.AddSingleton<TripService>();

//inkludera mubblazor
builder.Services.AddMudServices();

await builder.Build().RunAsync();
