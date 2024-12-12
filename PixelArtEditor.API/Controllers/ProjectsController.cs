using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelArtEditor.API.Models;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly PixelArtDBContext _context;

    public ProjectsController(PixelArtDBContext context)
    {
        _context = context;
    }

    // GET: api/projects
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
    {
        return await _context.Projects
            .Include(p => p.Frames)
            .ToListAsync();
    }

    // GET: api/projects/{id}/frames
    [HttpGet("{id}/frames")]
    public async Task<ActionResult<IEnumerable<Frame>>> GetProjectFrames(int id)
    {
        var frames = await _context.Frames
            .Include(f => f.Pixels)
            .Where(f => f.ProjectId == id)
            .ToListAsync();

        if (!frames.Any())
        {
            return NotFound();
        }

        return frames;
    }

    // POST: api/projects
    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
    }

    // POST: api/projects/{id}/frames
    [HttpPost("{id}/frames")]
    public async Task<ActionResult<Frame>> AddFrame(int id, Frame frame)
    {
        frame.ProjectId = id;
        _context.Frames.Add(frame);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProjectFrames), new { id = frame.Id }, frame);
    }

    // PUT: api/projects/frames/{id}
    [HttpPut("frames/{id}")]
    public async Task<IActionResult> UpdateFrame(int id, Frame frame)
    {
        if (id != frame.Id)
        {
            return BadRequest();
        }

        _context.Entry(frame).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Frames.Any(f => f.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/projects/frames/{id}
    [HttpDelete("frames/{id}")]
    public async Task<IActionResult> DeleteFrame(int id)
    {
        var frame = await _context.Frames.FindAsync(id);
        if (frame == null)
        {
            return NotFound();
        }

        var pixels = await _context.Pixels.Where(p => p.FrameId == id).ToListAsync();
        _context.Pixels.RemoveRange(pixels);

        _context.Frames.Remove(frame);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}