namespace JananAchekzai2.Client.Services.ClientPartyAccountServices;

public interface IClientPartyAccountService
{
    public Parties Party { get; set; }

    Task GetPartyAccount(string id);
    Task<bool> AddTransaction(Transactions transactions);
    Task<Transactions> GetSingleTransaction(string id);
    Task<bool> EditTransaction(Transactions transaction);
    Task DeleteTransaction(string id,string partyId);
}