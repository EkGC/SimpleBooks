using JananAchekzai2.Client.Services.PartyServices;
using JananAchekzai2.Server.Services.GenUniqueIdSerivices;
using JananAchekzai2.Server.Services.PartiesServices;
using JananAchekzai2.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace JannanAchekzay.Server.Controllers;

[Route("Api/[Controller]")]
[ApiController]
public class PartiesController : ControllerBase
{
    private readonly IPartiesService _partiesService;
    private readonly IGenUniqueIdService _genUniqueIdService;

    public PartiesController(IPartiesService partiesService, IGenUniqueIdService genUniqueIdService)
    {
        _partiesService = partiesService;
        _genUniqueIdService = genUniqueIdService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Parties>>> GetParties()
    {
        await _partiesService.GetParties();
        if (_partiesService.PartiesList.Count != 0)
            return Ok(_partiesService.PartiesList);
        return NotFound("no Data");
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Parties>> GetSingleParty(string id)
    {
        await _partiesService.GetSingleParty(id);
        if (_partiesService.Party is null)
        {
            return BadRequest("Could not Found Party");
        }
        return Ok(_partiesService.Party);
    }

    [HttpPost("AddParty")]
    public async Task<ActionResult> AddParty(Parties party)
    {
        party.Id = await _genUniqueIdService.GenerateId(party.Type);
        var result = await _partiesService.AddParty(party);
        if (result)
        {
            await _genUniqueIdService.AddId();
            return Ok("party added");   
        }
        
        return BadRequest("could not add the party");
    }

    [HttpPut("UpdateParty")]
    public async Task<ActionResult<bool>> UpdateParty(Parties party)
    {
        var result = await _partiesService.UpdateParty(party);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteParty(string id)
    {
        var result = await _partiesService.DeleteParty(id);
        if (result)
        {
            return Ok("Record Was Successfully Deleted");
        }
        return BadRequest("Could Not Delete The Party");
    }

    [HttpGet("GetPartyAccount/{id}")]
    public async Task<ActionResult<Parties>> GetPartyAccount(string id)
    {
        await _partiesService.GetPartyAccount(id);
        var result = _partiesService.Party;
        return Ok(result);
    }
}