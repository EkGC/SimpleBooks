global using JananAchekzai2.Shared.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using JananAchekzai2.Client;
using CurrieTechnologies.Razor.SweetAlert2;
using JananAchekzai2.Client.Services.ClientPartyAccountServices;
using JananAchekzai2.Client.Services.PartyServices;
using JananAchekzai2.Client.Services.StateContainerService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("JananAchekzai2.ServerAPI",
        client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("JananAchekzai2.ServerAPI"));

builder.Services.AddApiAuthorization();
builder.Services.AddScoped<IPartyService, PartyService>();
builder.Services.AddScoped<IClientPartyAccountService, ClientPartyAccountService>();
builder.Services.AddScoped<IStateContainerService, StateContainerService>();
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();