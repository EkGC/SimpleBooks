using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace JananAchekzai2.Client.Services.ClientPartyAccountServices;

public class ClientPartyAccountService:IClientPartyAccountService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    public Parties Party { get; set; }

    public ClientPartyAccountService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }
    public async Task GetPartyAccount(string id)
    {
        var result = await _httpClient.GetFromJsonAsync<Parties>($"api/parties/GetPartyAccount/{id}");
        if (result is not null)
        {
            Party = result;
        }
    }

    public async Task<bool> AddTransaction(Transactions transactions)
    {
        var result = await _httpClient.PostAsJsonAsync("Api/Transactions/AddTransaction",transactions);
        if (result.IsSuccessStatusCode)
        {
            _navigationManager.NavigateTo($"PartyAccount/{Party.Id}");
        }

        return false;
    }

    public async Task<Transactions> GetSingleTransaction(string id)
    {
        var result = await _httpClient.GetFromJsonAsync<Transactions>($"Api/Transactions/GetSingleTransaction/{id}");
        return result;
    }

    public async Task<bool> EditTransaction(Transactions transaction)
    {
        var result = await _httpClient.PutAsJsonAsync($"Api/Transactions/EditTransaction",transaction);
        if (result.IsSuccessStatusCode)
        {
            _navigationManager.NavigateTo($"PartyAccount/{transaction.PartyId}");
        }

        return false;
    }

    public async Task DeleteTransaction(string id, string partyId = "")
    {
        var result = await _httpClient
            .DeleteAsync($"Api/Transactions/DeleteTransaction/{id}");
        if (result.IsSuccessStatusCode)
        {
            _navigationManager.NavigateTo($"PartyAccount/{partyId}");
        }
    }
}