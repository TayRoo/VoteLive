using Xunit;
using Moq;
using VoteLive.Services;
using VoteLive.Services.Interfaces;
using VoteLive.Models;
using VoteLive.Repository;
using VoteLive.Hubs;
using VoteLive.Controller;
using VoteLive.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using VoteLive.Repository.Interfaces;



namespace VoteLive.Tests
{
    public class VoteServiceTests
    {
        private readonly Mock<IVoteRepository> _mockVoteRepository;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly VoteService _voteService;

        public VoteServiceTests()
        {
            _mockVoteRepository = new Mock<IVoteRepository>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _voteService = new VoteService(_mockVoteRepository.Object, _mockHttpClientFactory.Object);
        }

        [Fact]
        public async Task AddVote_ShouldSaveVoteToDatabase()
        {
            // Arrange
            var vote = new Vote { UserName = "TestUser", Choice = "Kaka" };
            _mockVoteRepository.Setup(repo => repo.AddVoteAsync(It.IsAny<Vote>())).ReturnsAsync(vote);

            // Act
            await _voteService.AddVoteAsync(vote);

            // Assert
            _mockVoteRepository.Verify(repo => repo.AddVoteAsync(It.IsAny<Vote>()), Times.Once);
            _mockVoteRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetVoteCounts_ShouldReturnCorrectCounts()
        {
            // Arrange
            var votes = new List<Vote>
            {
                new Vote { Choice = "Kaka" },
                new Vote { Choice = "CR7" },
                new Vote { Choice = "Messi" },
                new Vote { Choice = "Kaka" }
            };
            _mockVoteRepository.Setup(repo => repo.GetAllVotesAsync()).ReturnsAsync(votes);

            // Act
            var result = await _voteService.GetVoteCounts();

            // Assert
            Assert.Equal(2, result.numberOfVoteKaka);
            Assert.Equal(1, result.numberOfVoteCR7);
            Assert.Equal(1, result.numberOfVoteMessi);
        }

    }

    public class SignalRVoteTests
    {
        [Fact]
        public async Task SendVoteResult_ShouldSendToAllClients()
        {
            // Arrange
            var mockClients = new Mock<IHubCallerClients>();
            var mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub = new SignalRVote()
            {
                Clients = mockClients.Object
            };

            // Act
            await hub.SendVoteResult(1, 2, 3);

            // Assert
            mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "ReceiveVote",
                    It.Is<object[]>(o => (int)o[0] == 1 && (int)o[1] == 2 && (int)o[2] == 3),
                    default),
                Times.Once);
        }
    }
}