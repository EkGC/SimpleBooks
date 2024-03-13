using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace JananAchekzai2.Client.Services.PartyServices;

public class PartyService : IPartyService
{
    public List<Parties> PartiesList { get; set; } = new List<Parties>();
    public Parties party { get; set; }
    
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;

    public PartyService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }
    public async Task<bool> Add(Parties party)
    {
        var result = await _httpClient.PostAsJsonAsync("api/parties/AddParty", party);
        if (result.IsSuccessStatusCode)
        {
            _navigationManager.NavigateTo("PartiesView");
        }

        return result.IsSuccessStatusCode;
    }

    public async Task<List<Parties>> Get()
    {
        PartiesList = await _httpClient.GetFromJsonAsync<List<Parties>>("api/parties") ??
                      throw new InvalidOperationException();
        return PartiesList;
    }

    public async Task GetSingleParty(string id)
    {
        var result = await _httpClient.GetFromJsonAsync<Parties>($"api/parties/{id}");
        if (result is not null)
        {
            party = result;
        }
        
    }

    public async Task EditParty(Parties party)
    {
        var result = await _httpClient.PutAsJsonAsync("api/parties/UpdateParty", party);
        if (result.IsSuccessStatusCode)
        {
            _navigationManager.NavigateTo("PartiesView");
        }
    }

    public async Task<bool> DeleteParty(string id)
    {
        var result = await _httpClient.DeleteAsync($"api/parties/{id}");
        return result.IsSuccessStatusCode;
    }

    public async Task PartyAccount(string id)
    {
        party = await _httpClient.GetFromJsonAsync<Parties>($"api/parties/partyAccount/{id}");
    }
}