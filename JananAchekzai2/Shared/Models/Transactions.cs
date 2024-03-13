namespace JananAchekzai2.Shared.Models;

public class Transactions
{
    public string Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty;
    public int Ammount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime dateTime { get; set; } = DateTime.Now;
    public bool IsEdited { get; set; } = false;
    public string PartyId { get; set; }
    public Parties? Party { get; set; }
    
}