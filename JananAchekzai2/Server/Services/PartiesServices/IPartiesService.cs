

namespace JananAchekzai2.Server.Services.PartiesServices;

public interface IPartiesService
{
    public List<Parties> PartiesList { get; set; }
    public Parties Party { get; set; }

    Task GetParties();
    Task GetSingleParty(string id);
    Task<bool> AddParty(Parties party);
    Task<bool> UpdateParty(Parties party);
    Task<bool> DeleteParty(string id);
    Task GetPartyAccount(string id);
}