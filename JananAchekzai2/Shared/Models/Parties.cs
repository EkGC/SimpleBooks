using System.ComponentModel.DataAnnotations;

namespace JananAchekzai2.Shared.Models;

public class Parties
{
    public string Id { get; set; } = string.Empty;
   [Required]
    public string Name { get; set; } = string.Empty;
 [Required]
    public string LastName { get; set; } = string.Empty;
  [Required]
    public string Type { get; set; } = string.Empty;

    public int? OpeningBalanceTom { get; set; } = 0;
    public int? OpeningBalanceAfg { get; set; } = 0;
    public int? BalanceTom { get; set; } = 0;
    public int? BalanceAfg { get; set; } = 0;
    public List<Transactions>? TransactionsList { get; set; } = new List<Transactions>();
}