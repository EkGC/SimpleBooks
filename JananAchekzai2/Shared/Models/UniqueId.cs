using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Principal;

namespace JananAchekzai2.Shared.Models;

public class UniqueId
{
    public int Id { get; set; }
    public string GeneratedIds { get; set; }
    public string Type { get; set; } = string.Empty;
}