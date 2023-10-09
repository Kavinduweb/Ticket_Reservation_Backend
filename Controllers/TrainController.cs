using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketReservation.Models;
using TicketReservation.Services;

namespace TicketReservation.Controllers
{
   [Route("api/trains")]
[ApiController]
public class TrainController : ControllerBase
{
    private readonly TrainService _trainService;

    public TrainController(TrainService trainService)
    {
        _trainService = trainService;
    }

    [HttpGet]
    public async Task<List<Train>> Get()
    {
        return await _trainService.GetTrainsAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Train>> Get(string id)
    {
        var train = await _trainService.GetTrainByIdAsync(id);
        if (train == null)
        {
            return NotFound();
        }
        return Ok(train);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Train train)
    {
        await _trainService.CreateAsync(train);
        return CreatedAtAction(nameof(Get), new { id = train.Id }, train);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] Train train)
    {
        await _trainService.UpdateTrainAsync(id, train);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _trainService.DeleteAsync(id);
        return NoContent();
    }
}
}