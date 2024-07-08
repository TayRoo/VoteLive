using VoteLive.Models;
using VoteLive.Repository;
using VoteLive.Services.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using VoteLive.Repository.Interfaces;

namespace VoteLive.Services
{
    public class VoteService : IVoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public VoteService(IVoteRepository voteRepository, IHttpClientFactory httpClientFactory)
        {
            _voteRepository = voteRepository ?? throw new ArgumentNullException(nameof(voteRepository));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task PostVoteAsync(Vote vote)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            await httpClient.PostAsJsonAsync("api/PostVote", vote);
        }
        public async Task<List<Vote>> GetAllVotesAsync()
        {
            using var httpClient = _httpClientFactory.CreateClient();
            var voteList = await httpClient.GetFromJsonAsync<List<Vote>>("api/GetVotes");
            return voteList ?? new List<Vote>();
        }
        // add it in DB
        public async Task<Vote> AddVoteAsync(Vote vote)
        {
            await _voteRepository.AddVoteAsync(vote);
            await _voteRepository.SaveChangesAsync();
            return vote;
        }

        public async Task<(int numberOfVoteKaka, int numberOfVoteCR7, int numberOfVoteMessi)> GetVoteCounts()
        {
            var votes = await _voteRepository.GetAllVotesAsync();
            int kakaCount = votes.Count(v => v.Choice == "Kaka");
            int cr7Count = votes.Count(v => v.Choice == "CR7");
            int messiCount = votes.Count(v => v.Choice == "Messi");
            return (kakaCount, cr7Count, messiCount);
        }
    }
}
