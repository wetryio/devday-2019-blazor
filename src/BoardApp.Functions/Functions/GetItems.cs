using BoardApp.Functions.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace BoardApp.Functions
{
    public class GetItems
    {
        private readonly IBoardItemRepository boardItemRepository;

        public GetItems(IBoardItemRepository boardItemRepository)
        {
            this.boardItemRepository = boardItemRepository;
        }

        [FunctionName("GetItems")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "task")] HttpRequest req)
        {
            return new OkObjectResult(await boardItemRepository.ReadAll());
        }
    }
}
