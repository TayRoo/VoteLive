using Microsoft.EntityFrameworkCore;
using VoteLive.Models;
using VoteLive.Repository.Interfaces;


namespace VoteLive.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vote> Votes { get; set; }
        // Add other DbSet properties for your entities here
    }
    public class VoteRepository : DbContext, IVoteRepository
    {
        private readonly ApplicationDbContext _context;
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //: base(options)
        //{
        //}

        public VoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Vote> AddVoteAsync(Vote vote)
        {
            await _context.Votes.AddAsync(vote);
            return vote;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        // Define your DbSets here. For example:
        //public DbSet<Vote> Votes { get; set; }
        public async Task<List<Vote>> GetAllVotesAsync()
        {
            return await _context.Votes.ToListAsync();
        }

    }
}
