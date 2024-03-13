namespace JananAchekzai2.Client.Services.PartyServices;

public interface IPartyService
{
    public List<Parties> PartiesList { get;}
    public Parties party { get; set; }
    Task<bool> Add(Parties party);
    Task<List<Parties>> Get();
    Task GetSingleParty(string id);
    Task EditParty(Parties party);
    Task<bool> DeleteParty(string id);
    Task PartyAccount(string id);
}