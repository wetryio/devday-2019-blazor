using BoardApp.Functions.Infrastructure;
using BoardApp.Functions.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(BoardApp.Functions.Startup))]
namespace BoardApp.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string SqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<BoardContext>(
                options => options.UseSqlServer(SqlConnection));

            builder.Services.AddScoped<IBoardItemRepository, BoardItemRepository>();
        }
    }
}
