using BoardApp.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoardApp.Shared.Services
{
    public interface IBoardItemService
    {
        Task<BoardItem[]> GetBoardItems();
        Task<BoardItem> GetBoardItem(Guid id);
        Task<bool> CreateBoardItem(BoardItem item);
        Task<bool> UpdateBoardItem(BoardItem item);
    }

    public class BoardItemService : IBoardItemService
    {
        public async Task<bool> CreateBoardItem(BoardItem item)
        {
            try
            {
                var itemResponse = await GetHttpClient().PostAsync(string.Empty, GetItemAsContent(item));

                return itemResponse.IsSuccessStatusCode;
            }
            catch (Exception)
            {

            }

            return false;
        }

        public async Task<BoardItem> GetBoardItem(Guid id)
        {
            try
            {
                var itemResponse = await GetHttpClient().GetAsync($"{id}");

                if (itemResponse.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<BoardItem>(await itemResponse.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<BoardItem[]> GetBoardItems()
        {
            try
            {
                var itemResponse = await GetHttpClient().GetAsync(string.Empty);

                if (itemResponse.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<BoardItem[]>(await itemResponse.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<bool> UpdateBoardItem(BoardItem item)
        {
            try
            {
                var itemResponse = await GetHttpClient().PutAsync($"{item?.Id}", GetItemAsContent(item));

                return itemResponse.IsSuccessStatusCode;
            }
            catch (Exception)
            {

            }

            return false;
        }

        private HttpContent GetItemAsContent(BoardItem item)
        {
            var itemJson = JsonConvert.SerializeObject(item);
            return new StringContent(itemJson, Encoding.UTF8, "application/json");
        }

        private HttpClient GetHttpClient()
        {
            return new HttpClient()
            {
                BaseAddress = new Uri("https://boardappfunctions.azurewebsites.net/api/task/")
            };
        }
    }
}
