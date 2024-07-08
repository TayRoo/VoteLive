using Microsoft.EntityFrameworkCore;
using VoteLive.Models;

namespace VoteLive.Repository.Interfaces
{

        public interface IVoteRepository
    {
            Task<Vote> AddVoteAsync(Vote vote);
            Task SaveChangesAsync();
            Task<List<Vote>> GetAllVotesAsync();
        }
    
}
