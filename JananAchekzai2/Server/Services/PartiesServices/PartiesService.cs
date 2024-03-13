using Microsoft.EntityFrameworkCore;

namespace JananAchekzai2.Server.Services.PartiesServices;

public class PartiesService : IPartiesService
{
    public List<Parties> PartiesList { get; set; } = new List<Parties>();
    public Parties Party { get; set; } = new Parties(); 
    
    private readonly ApplicationDbContext _context;

    public PartiesService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task GetParties()
    {
        PartiesList = await _context.Parties.ToListAsync();
    }

    public async Task GetSingleParty(string id)
    {
        var result = await _context.Parties
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
        if (result is not null)
        {
            Party = result;
        }
    }

    public async Task<bool> AddParty(Parties party)
    {
        _context.Parties.Add(party);
        var result = await _context.SaveChangesAsync();
        return (result > 0);
    }

    public async Task<bool> UpdateParty(Parties party)
    {
        var result = await _context.Parties.FirstOrDefaultAsync(p => p.Id == party.Id);
        result.Type = party.Type;
        result.Name = party.Name;
        result.LastName = party.LastName;
        if (party.OpeningBalanceAfg != result.OpeningBalanceAfg)
        {
            if (party.OpeningBalanceAfg>result.OpeningBalanceAfg)
            {
                var newBalanceAfg = party.OpeningBalanceAfg - result.OpeningBalanceAfg;
                result.BalanceAfg += newBalanceAfg;
            }else if (party.OpeningBalanceAfg < result.OpeningBalanceAfg)
            {
                var newBalance = result.OpeningBalanceAfg - party.OpeningBalanceAfg;
                result.BalanceAfg -= newBalance;
            }
        }
        result.OpeningBalanceAfg = party.OpeningBalanceAfg;
        result.OpeningBalanceTom = party.OpeningBalanceTom;

        _context.Parties.Update(result);
       return await _context.SaveChangesAsync()>0;
    }

    public async Task<bool> DeleteParty(string id)
    {
        var result =  _context.Parties
            .FirstOrDefault(p =>p.Id == id);
        _context.Parties.Remove(result);
        var SaveResult = await _context.SaveChangesAsync();
        return SaveResult != 0;
    }

    public async Task GetPartyAccount(string id)
    {
        var result = await _context.Parties
            .Where(p=>p.Id == id)
            .Include(p => p.TransactionsList)
            .FirstOrDefaultAsync();
        if (result is not null)
        {
            Party = result;
        }
    }
}