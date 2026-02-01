using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Shared.Entities;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimeEntriesController : ControllerBase
{
    private readonly TaskFlowDbContext _context;
    private readonly ILogger<TimeEntriesController> _logger;

    public TimeEntriesController(TaskFlowDbContext context, ILogger<TimeEntriesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/timeentries/task/5
    [HttpGet("task/{taskId}")]
    public async Task<ActionResult<IEnumerable<TimeEntry>>> GetTimeEntriesByTask(int taskId)
    {
        var timeEntries = await _context.TimeEntries
            .Where(te => te.TaskItemId == taskId)
            .Include(te => te.TaskItem)
            .ToListAsync();
        
        return Ok(timeEntries);
    }

    // POST: api/timeentries
    [HttpPost]
    public async Task<ActionResult<TimeEntry>> CreateTimeEntry(TimeEntry timeEntry)
    {
        // Verify task exists
        var task = await _context.TaskItems.FindAsync(timeEntry.TaskItemId);
        if (task == null)
        {
            return NotFound($"Task with ID {timeEntry.TaskItemId} not found.");
        }

        // Calculate duration if not set
        if (timeEntry.Duration == TimeSpan.Zero && timeEntry.EndTime > timeEntry.StartTime)
        {
            timeEntry.Duration = timeEntry.EndTime - timeEntry.StartTime;
        }

        _context.TimeEntries.Add(timeEntry);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTimeEntry), new { id = timeEntry.Id }, timeEntry);
    }

    // GET: api/timeentries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TimeEntry>> GetTimeEntry(int id)
    {
        var timeEntry = await _context.TimeEntries
            .Include(te => te.TaskItem)
            .FirstOrDefaultAsync(te => te.Id == id);

        if (timeEntry == null)
        {
            return NotFound();
        }

        return Ok(timeEntry);
    }

    // PUT: api/timeentries/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTimeEntry(int id, TimeEntry timeEntry)
    {
        if (id != timeEntry.Id)
        {
            return BadRequest();
        }

        _context.Entry(timeEntry).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TimeEntryExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/timeentries/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTimeEntry(int id)
    {
        var timeEntry = await _context.TimeEntries.FindAsync(id);
        if (timeEntry == null)
        {
            return NotFound();
        }

        _context.TimeEntries.Remove(timeEntry);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TimeEntryExists(int id)
    {
        return _context.TimeEntries.Any(e => e.Id == id);
    }
}
