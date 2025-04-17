using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveRequestsController : ControllerBase
{
    private readonly ILeaveRequestService _service;

    public LeaveRequestsController(ILeaveRequestService service)
    {
        _service = service;
    }

    // GET: api/leaverequests
    [HttpGet]
    public async Task<ActionResult<List<LeaveRequestDto>>> GetAll()
    {
        var leaves = await _service.GetAllAsync();
        return Ok(leaves);
    }

    // GET: api/leaverequests/5
    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveRequestDto>> GetById(int id)
    {
        var leave = await _service.GetByIdAsync(id);
        if (leave == null)
            return NotFound();

        return Ok(leave);
    }

    // POST: api/leaverequests
    [HttpPost]
    public async Task<ActionResult<LeaveRequestDto>> Create([FromBody] LeaveRequestDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/leaverequests/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LeaveRequestDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch.");

        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    // DELETE: api/leaverequests/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    // GET: api/leaverequests/filter
    [HttpGet("filter")]
    public async Task<ActionResult<List<LeaveRequestDto>>> Filter([FromQuery] LeaveRequestFilterDto filter)
    {
        var result = await _service.GetFilteredAsync(filter);
        return Ok(result);
    }

    // PUT: api/leaverequests/{id}/approve
    [HttpPut("{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        try
        {
            await _service.ApproveAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: api/leaverequests/report?year=2023&department=IT&startDate=2023-01-01&endDate=2023-12-31
    [HttpGet("report")]
    public async Task<ActionResult<List<LeaveReportDto>>> GetReport(
        [FromQuery] int year,
        [FromQuery] string? department,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var result = await _service.GetReportAsync(year, department, startDate, endDate);
        return Ok(result);
    }

}
