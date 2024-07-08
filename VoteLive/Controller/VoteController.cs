using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoteLive.Models;
using VoteLive.Repository;

namespace VoteLive.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostVote(Vote vote)
        {
            if (vote == null)
            {
                return BadRequest();
            }

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVote), new { id = vote.Id }, vote);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVote(int id)
        {
            var vote = await _context.Votes.FindAsync(id);

            if (vote == null)
            {
                return NotFound();
            }

            return Ok(vote);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vote>>> GetVotes()
        {
            var votes = await _context.Votes.ToListAsync();
            return Ok(votes);
        }
    }
}
