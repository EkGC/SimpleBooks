namespace JananAchekzai2.Client.Services.StateContainerService;

public interface IStateContainerService
{
    public event Action? State;
    public Parties Party { get; set; }
    void PartyData(Parties partyData);
}