using BoardApp.Functions.Repositories;
using BoardApp.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace BoardApp.Functions
{
    public class CreateItem
    {
        private readonly IBoardItemRepository boardItemRepository;

        public CreateItem(IBoardItemRepository boardItemRepository)
        {
            this.boardItemRepository = boardItemRepository;
        }

        [FunctionName("CreateItem")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "task")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<BoardItem>(requestBody);

            var itemCreationResult = await boardItemRepository.Insert(model);

            return itemCreationResult ? (IActionResult)new CreatedResult(string.Empty, new { Success = itemCreationResult }) : (IActionResult)new BadRequestObjectResult(new { });
        }
    }
}
