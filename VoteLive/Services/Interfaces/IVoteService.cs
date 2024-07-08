using VoteLive.Models;

namespace VoteLive.Services.Interfaces
{
    public interface IVoteService
    {
        Task PostVoteAsync(Vote vote);
        Task<List<Vote>> GetAllVotesAsync();
        Task<Vote> AddVoteAsync(Vote vote);
        Task<(int numberOfVoteKaka, int numberOfVoteCR7, int numberOfVoteMessi)> GetVoteCounts();
    }
}
