using BoardApp.Functions.Repositories;
using BoardApp.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BoardApp.Functions
{
    public class UpdateItem
    {
        private readonly IBoardItemRepository boardItemRepository;

        public UpdateItem(IBoardItemRepository boardItemRepository)
        {
            this.boardItemRepository = boardItemRepository;
        }

        [FunctionName("UpdateItem")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "task/{taskId}")] HttpRequest req,
            [FromRoute]Guid taskId)
        {

            var taskToUpdate = await boardItemRepository.Read(taskId);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<BoardItem>(requestBody);

            taskToUpdate.Status = model.Status;
            taskToUpdate.Points = model.Points;
            taskToUpdate.Description = model.Description;
            taskToUpdate.Summary = model.Summary;

            var itemUpdateResult = await boardItemRepository.Update(taskToUpdate);

            return itemUpdateResult ? (IActionResult)new NoContentResult() : (IActionResult)new BadRequestObjectResult(new { });

        }
    }
}
