using Microsoft.AspNetCore.SignalR;
using VoteLive.Models;
using VoteLive.Repository;
using VoteLive.Services.Interfaces;

namespace VoteLive.Hubs
{
    public class SignalRVote : Hub
    {

        public async Task SendVoteResult( int numberOfVoteKaka, int numberOfVoteCR7, int numberOfVoteMessi)
        {
            await Clients.All.SendAsync("ReceiveVote",  numberOfVoteKaka, numberOfVoteCR7, numberOfVoteMessi);

        }
    }
}
