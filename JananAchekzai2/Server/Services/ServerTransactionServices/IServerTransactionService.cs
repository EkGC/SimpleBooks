using System.Transactions;

namespace JananAchekzai2.Server.Services.ServerTransactionServices;

public interface IServerTransactionService
{
    public List<Transactions> TransactionsList { get; set; }
    Task GetTransactions();
    Task<Transactions> GetSingleTransaction(string id);
    Task<bool> AddTransaction(Transactions transactions);
    Task<Transactions> UpdateTransaction(Transactions transactions);
    Task<bool> DeleteTransaction(string id);
}