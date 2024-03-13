using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace JananAchekzai2.Server.Services.GenUniqueIdSerivices;

public class GenUniqueIdSerivice : IGenUniqueIdService
{
    private readonly ApplicationDbContext _context;
    public GenUniqueIdSerivice(ApplicationDbContext context)
    {
        _context = context;
    }

    
    string newId = string.Empty;
    UniqueId uniqueId = new();
    public async Task<string> GenerateId(string type)
    {
        int Num;
        var UID = new UniqueId();
            UID =  await _context.UniqueIds
                        .Where(u => u.Type == type)
                        .OrderByDescending(o => o.GeneratedIds)
                        .FirstOrDefaultAsync();
        
        Num = UID == null ? 0 : ExtractNumber(UID.GeneratedIds);
        
        if (type == "Customer")
        {
            if (Num+1 < 10)
                newId = "Cus000" + (Num + 1);
            else if (Num+1 < 100)
                newId = "Cus00" + (Num + 1);
            else if (Num+1 < 1000)
                newId = "Cus0" + (Num + 1);
            else if (Num+1 >= 1000) newId = "Cus" + (Num + 1);
            uniqueId.Type = type;
            uniqueId.GeneratedIds = newId;
        }
        if (type == "Transaction")
        {
            if (Num+1 < 10)
                newId = "Tran000" + (Num + 1);
            else if (Num+1 < 100)
                newId = "Tran00" + (Num + 1);
            else if (Num < 1000)
                newId = "Tran0" + (Num + 1);
            else if (Num >= 1000) newId = "Tran" + (Num + 1);
            uniqueId.Type = type;
            uniqueId.GeneratedIds = newId;
        }

        return newId;
    }

    public async Task AddId()
    {
        _context.UniqueIds.Add(uniqueId);
        await _context.SaveChangesAsync();
    }

    private int ExtractNumber(string value)
    {
        Regex regex = new Regex(@"[0-9]+");
        var matches = regex.Match(value);

        return int.Parse(matches.Value);
    }
}