namespace JananAchekzai2.Client.Services.StateContainerService;

public class StateContainerService : IStateContainerService
{
    public event Action? State;
    public Parties Party { get; set; }
    public void PartyData(Parties partyData)
    {
        Party = partyData;
        State?.Invoke();
    }
}