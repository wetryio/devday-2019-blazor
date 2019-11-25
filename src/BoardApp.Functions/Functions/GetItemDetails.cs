using BoardApp.Functions.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Threading.Tasks;

namespace BoardApp.Functions.Functions
{
    public class GetItemDetails
    {
        private readonly IBoardItemRepository boardItemRepository;

        public GetItemDetails(IBoardItemRepository boardItemRepository)
        {
            this.boardItemRepository = boardItemRepository;
        }

        [FunctionName("GetItemDetails")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "task/{taskId}")] HttpRequest req,
            [FromRoute] Guid taskId)
        {
            var item = await boardItemRepository.Read(taskId);

            return item == null ? (IActionResult)new NotFoundObjectResult(null) : new OkObjectResult(await boardItemRepository.Read(taskId));
        }
    }
}
