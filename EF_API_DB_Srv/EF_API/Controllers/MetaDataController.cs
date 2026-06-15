using EF_API.Services;
using Microsoft.AspNetCore.Mvc;
using model = EF_API.Models;
using service = EF_API.Services;

namespace EF_API.Controllers;

[ApiController]
[Route("api/metadata")]
public class MetaDataController : ControllerBase
{
    /*private readonly EF_API_Services _apiServices;



    public MetaDataController(EF_API_Services p_services)
    {
        this._apiServices = p_services;
    }*/

    private readonly service.Array _arrayService;
    private readonly service.Description _descriptionService;
    private readonly service.Note _noteService;
    private readonly service.Historic _historicService;

    private const int MIN = 1;
    private const int MAX = 4;

    public MetaDataController(
        service.Array arrayService,
        service.Description descriptionService,
        service.Note noteService,
        service.Historic historicService)
    {
        _arrayService = arrayService;
        _descriptionService = descriptionService;
        _noteService = noteService;
        _historicService = historicService;
    }

    [HttpGet("arrays")]
    [ProducesResponseType(200)]
    public ActionResult<IEnumerable<model.Array>> GetArrays([FromQuery] int selection)
    {
        if (selection < MIN || selection > MAX) return BadRequest($"Must be between {MIN} and {MAX}");

        var result = _arrayService.GetFiltereds(selection);
        return Ok(result);
    }

    [HttpGet("descriptions")]
    [ProducesResponseType(200)]
    public ActionResult<IEnumerable<model.Description>> GetDescriptions([FromQuery] int selection)
    {
        if (selection < MIN || selection > MAX) return BadRequest($"Must be between {MIN} and {MAX}");

        var data = _descriptionService.GetFiltereds(selection);
        return Ok(data);
    }

    [HttpGet("notes")]
    [ProducesResponseType(200)]
    public ActionResult<IEnumerable<model.Note>> GetNotes([FromQuery] int selection)
    {
        var data = _noteService.GetFiltereds(selection);
        return Ok(data);
    }

    #region Historic

    [HttpGet("historic")]
    [ProducesResponseType(200)]
    public ActionResult<Models.Historic?> GetHistoric(
        [FromQuery] string userId,
        [FromQuery] string prnSelection)
    {
        var result = _historicService.GetByKey(userId, prnSelection);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("historic")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public ActionResult UpdateHistoric([FromBody] Models.Historic historic)
    {
        var existing = _historicService.GetByKey(historic.UserId, historic.PRN_Selection);
        if (existing == null) return NotFound();

        _historicService.Update(historic);
        return NoContent(); // 204 — standard REST pour un PUT réussi
    }

    [HttpPost("historic")]
    [ProducesResponseType(201)]
    public ActionResult<Models.Historic> SaveHistoric([FromBody] Models.Historic historic)
    {
        
        Console.WriteLine($"[DEBUG POST] userId='{historic.UserId}' | prn='{historic.PRN_Selection}'");
        
        var result = _historicService.Save(historic);
        return CreatedAtAction(nameof(GetHistoric),
            new { userId = result.UserId, prnSelection = result.PRN_Selection }, result);
    }
    #endregion
}