namespace JananAchekzai2.Server.Services.GenUniqueIdSerivices;

public interface IGenUniqueIdService
{
    // public int Num { get; set; }
    // public string Str { get; set; }
    // public string newId { get; set; }

    Task<string> GenerateId(string type);
    Task AddId();
}