using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Xunit;
using VoteLive.Models;
using VoteLive.Repository;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace VoteLive.Tests.Integration
{
    public class VoteControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public VoteControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });
                });
            });
        }

        [Fact]
        public async Task PostVote_ShouldReturnOkResult()
        {
            // Arrange
            var client = _factory.CreateClient();
            var vote = new Vote { UserName = "TestUser", Choice = "Kaka" };

            // Act
            var response = await client.PostAsJsonAsync("/api/vote", vote);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("Created", response.StatusCode.ToString());
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetVoteCounts_ShouldReturnCorrectCounts()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/vote/counts");
            var counts = await response.Content.ReadFromJsonAsync<VoteCountsResult>();

            // Assert
            Assert.NotNull(counts);
            Assert.True(counts.numberOfVoteKaka >= 0);
            Assert.True(counts.numberOfVoteCR7 >= 0);
            Assert.True(counts.numberOfVoteMessi >= 0);
        }
    }
}