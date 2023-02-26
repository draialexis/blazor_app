using blazor_lab.Components;
using blazor_lab.Factories;
using blazor_lab.Models;

namespace blazor_lab.Services
{
    public class DataApiService : IDataService
    {
        private readonly HttpClient _http;
        private readonly string _apiBaseUrl;

        public DataApiService(
            HttpClient http,
            IConfiguration config)
        {
            _http = http;
            _apiBaseUrl = config.GetSection("CraftingApi")["BaseUrl"];
        }

        public async Task Add(ItemModel model)
        {
            // Get the item
            var item = ItemFactory.Create(model);

            // Save the data
            await _http.PostAsJsonAsync($"{_apiBaseUrl}/", item);
        }

        public async Task<int> Count()
        {
            return await _http.GetFromJsonAsync<int>($"{_apiBaseUrl}/count");
        }

        public async Task<List<Item>> All()
        {
            return await _http.GetFromJsonAsync<List<Item>>($"{_apiBaseUrl}/all");
        }

        public async Task<List<Item>> List(int currentPage, int pageSize)
        {
            return await _http.GetFromJsonAsync<List<Item>>($"{_apiBaseUrl}/?currentPage={currentPage}&pageSize={pageSize}");
        }

        public async Task<Item> GetById(int id)
        {
            return await _http.GetFromJsonAsync<Item>($"{_apiBaseUrl}/{id}");
        }

        public async Task Update(int id, ItemModel model)
        {
            // Get the item
            var item = ItemFactory.Create(model);

            await _http.PutAsJsonAsync($"{_apiBaseUrl}/{id}", item);
        }

        public async Task Delete(int id)
        {
            await _http.DeleteAsync($"{_apiBaseUrl}/{id}");
        }

        public async Task<List<CraftingRecipe>> GetRecipes()
        {
            return await _http.GetFromJsonAsync<List<CraftingRecipe>>($"{_apiBaseUrl}/recipe");
        }
    }
}
