using System.Transactions;
using JananAchekzai2.Server.Services.GenUniqueIdSerivices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JananAchekzai2.Server.Services.ServerTransactionServices;

public class ServerTransactionService:IServerTransactionService
{
    private readonly IGenUniqueIdService _genUniqueIdService;
    private readonly ApplicationDbContext _dataContext;
    public List<Transactions> TransactionsList { get; set; } = new();

    public ServerTransactionService(IGenUniqueIdService genUniqueIdService, ApplicationDbContext dataContext)
    {
        _genUniqueIdService = genUniqueIdService;
        _dataContext = dataContext;
    }
    public async Task GetTransactions()
    {
        var result = await _dataContext.Transactions.ToListAsync();
        if (result.Count != 0)
        {
            TransactionsList = result;
        }
    }

    public async Task<Transactions> GetSingleTransaction(string id)
    {
        var result = await _dataContext.Transactions
            .Include(t =>t.Party)
            .FirstOrDefaultAsync(t=>t.Id == id);
        return result;
    }

    public async Task<bool> AddTransaction(Transactions transactions)
    {
        transactions.Party = null;
        transactions.Id = await _genUniqueIdService.GenerateId("Transaction");
        _dataContext.Transactions.Add(transactions);
        await _dataContext.SaveChangesAsync();
        var partyResult = await _dataContext.Parties.Where(p => p.Id == transactions.PartyId)
            .FirstOrDefaultAsync();
        if (partyResult.Id == transactions.PartyId)
        {
            if (transactions.Currency == "Afg")
            {
                if (transactions.TransactionType =="Credit")
                {
                    partyResult.BalanceAfg += transactions.Ammount;
                }else if (transactions.TransactionType == "Debit")
                {
                    var negInt = transactions.Ammount * (-1);
                    partyResult.BalanceAfg += negInt;
                }
            }else if (transactions.Currency == "Toman")
            {
                if (transactions.TransactionType == "Credit")
                {
                    partyResult.BalanceTom += transactions.Ammount;
                }else if (transactions.TransactionType == "Debit")
                {
                    var negInt = transactions.Ammount * (-1);
                    partyResult.BalanceTom += negInt;
                }
            }
            _dataContext.Parties.Update(partyResult);
            var saveResult = await _dataContext.SaveChangesAsync();
            if (saveResult>0)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<Transactions> UpdateTransaction(Transactions transaction)
    {
        var result = await _dataContext.Transactions
            .Include(t => t.Party)
            .FirstOrDefaultAsync(p => p.Id == transaction.Id);
        if (result.IsEdited)
        {
            return null;
        }

        if (transaction.TransactionType == "Debit" && result.TransactionType == "Debit")
        {
            if (transaction.Currency == "Afg")
            {
                if (transaction.Ammount> result.Ammount)
                {
                    result.Party.BalanceAfg -= Math.Abs(transaction.Ammount - result.Ammount);
                }
                else
                {
                    result.Party.BalanceAfg += Math.Abs(transaction.Ammount - result.Ammount);
                }
               
            }else if (transaction.Currency == "Toman")
            {
                result.Party.BalanceTom -= Math.Abs(transaction.Ammount - result.Ammount);
            }
        }else if (transaction.TransactionType == "Credit" && result.TransactionType == "Credit")
        {
            if (transaction.Currency == "Afg")
            {
                result.Party.BalanceAfg += Math.Abs(transaction.Ammount - result.Ammount);
            }else if (transaction.Currency == "Toman")
            {
                result.Party.BalanceTom += Math.Abs(transaction.Ammount - result.Ammount);
            }
        }else if (transaction.TransactionType!=result.TransactionType)
        {
            if (transaction.TransactionType == "Credit" && transaction.Ammount == result.Ammount && transaction.Currency == "Afg")
            {
                result.Party.BalanceAfg += transaction.Ammount * 2;
            }else if (transaction.TransactionType == "Debit" && transaction.Ammount == result.Ammount && transaction.Currency == "Afg")
            {
                result.Party.BalanceAfg -= transaction.Ammount * 2;
            }
            if (transaction.TransactionType == "Credit" && transaction.Ammount == result.Ammount && transaction.Currency == "Toman")
            {
                result.Party.BalanceTom += transaction.Ammount * 2;
            }else if (transaction.TransactionType == "Debit" && transaction.Ammount == result.Ammount && transaction.Currency == "Toman")
            {
                result.Party.BalanceTom -= transaction.Ammount * 2;
            }
            if (transaction.Ammount!= result.Ammount)
            {
                if (transaction is { TransactionType: "Credit", Currency: "Afg" })
                {
                    result.Party.BalanceAfg += result.Ammount;
                    result.Party.BalanceAfg += transaction.Ammount;
                }else if (transaction is {TransactionType:"Debit", Currency:"Afg"})
                {
                    result.Party.BalanceAfg -= result.Ammount;
                    result.Party.BalanceAfg -= transaction.Ammount;
                }
                if (transaction is { TransactionType: "Credit", Currency: "Toman" })
                {
                    result.Party.BalanceTom += result.Ammount;
                    result.Party.BalanceTom += transaction.Ammount;
                }else if (transaction is {TransactionType:"Debit", Currency:"Toman"})
                {
                    result.Party.BalanceTom -= result.Ammount;
                    result.Party.BalanceTom -= transaction.Ammount;
                }
            }
        }
        else
        {
            return null;
        }
        result.Ammount = transaction.Ammount;
        result.Currency = transaction.Currency;
        result.TransactionType = transaction.TransactionType;
        result.IsEdited = true;
        _dataContext.Transactions.Update(result);
        var updateResult = await _dataContext.SaveChangesAsync();
        return updateResult > 0 ? result : null;
    }

    public async Task<bool> DeleteTransaction(string id)
    {

        var transaction = await _dataContext.Transactions
            .Where(t => t.Id == id)
            .Include(t => t.Party)
            .FirstOrDefaultAsync();
        var partyAccount = transaction.Party;
        switch (transaction.TransactionType)
        {
            case "Credit" when transaction.Currency == "Afg" :
                partyAccount.BalanceAfg -= transaction.Ammount;
                break;
            case "Credit":
                partyAccount.BalanceTom -= transaction.Ammount;
                break;
            case "Debit" when transaction.Currency == "Afg":
                partyAccount.BalanceAfg += transaction.Ammount;
                break;
            case "Debit":
                partyAccount.BalanceTom += transaction.Ammount;
                break;
        }
        _dataContext.Parties.Update(partyAccount);
        var deleteResult = await _dataContext.SaveChangesAsync();
        _dataContext.Transactions.Remove(transaction);
        deleteResult += await _dataContext.SaveChangesAsync();
        return deleteResult > 0;
    }
}