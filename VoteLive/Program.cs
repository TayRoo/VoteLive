using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VoteLive.Hubs;
using VoteLive.Repository;
using VoteLive.Services.Interfaces;
using VoteLive.Services;
using VoteLive.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddControllers();  // Enable controllers
builder.Services.AddHttpClient();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register the vote service
builder.Services.AddScoped<IVoteRepository, VoteRepository>();

builder.Services.AddScoped<IVoteService, VoteService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<SignalRVote>("/chatHub"); // later we need to change it 
app.MapControllers();  // Map controller routes

app.Run();
public partial class Program { }