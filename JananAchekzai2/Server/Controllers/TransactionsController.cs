using System.Transactions;
using JananAchekzai2.Server.Services.GenUniqueIdSerivices;
using JananAchekzai2.Server.Services.PartiesServices;
using JananAchekzai2.Server.Services.ServerTransactionServices;
using Microsoft.AspNetCore.Mvc;

namespace JannanAchekzay.Server.Controllers;

[Route("Api/[Controller]")]
[ApiController]
public class TransactionsController:ControllerBase
{
    private readonly IServerTransactionService _serverTransactionService;
    private readonly IGenUniqueIdService _genUniqueIdService;

    public TransactionsController(IServerTransactionService serverTransactionService, IGenUniqueIdService genUniqueIdService)
    {
        _serverTransactionService = serverTransactionService;
        _genUniqueIdService = genUniqueIdService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Transactions>>> GetTransactions()
    {
        await _serverTransactionService.GetTransactions();
        var result =  _serverTransactionService.TransactionsList;
        if (result.Count !=0)
        {
            return Ok(result);  
        }else if (result.Count == 0)
        {
            return Ok(result = new List<Transactions>());
        }

        return BadRequest("Sorry No Data Was Found");
    }

    [HttpPost("AddTransaction")]
    public async Task<ActionResult<bool>> AddTransaction(Transactions transactions)
    {
        var result = await _serverTransactionService.AddTransaction(transactions);
        if (result)
        {
            await _genUniqueIdService.AddId();
            return Ok("Party Transaction Was Added Successfully");
        }

        return BadRequest("Could Not Update the Party Transaction");
    }

    [HttpGet("GetSingleTransaction/{id}")]
    public async Task<ActionResult<Transactions>> GetSingleTransaction(string id)
    {
        var result = await _serverTransactionService.GetSingleTransaction(id);
        if (result is not null)
        {
            return Ok(result);
        }

        return BadRequest("could not found the transaction");
    }

    [HttpPut("EditTransaction")]
    public async Task<ActionResult<Transactions>> EditTransaction(Transactions transactions)
    {
        var result = await _serverTransactionService.UpdateTransaction(transactions);
        if (result is not null)
        {
            return Ok(result);
        }

        return BadRequest("Can not edite the record");
    }

    [HttpDelete("DeleteTransaction/{id}")]
    public async Task<ActionResult> DeleteTransaction(string id)
    {
        var result = await _serverTransactionService.DeleteTransaction(id);
        if (result)
        {
            return Ok();
        }

        return NotFound();
    }
}