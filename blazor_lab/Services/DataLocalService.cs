using blazor_lab.Components;
using blazor_lab.Factories;
using blazor_lab.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace blazor_lab.Services
{
    public class DataLocalService : IDataService
    {


        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly NavigationManager _navigationManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string pathToFakeData;

        public DataLocalService(
            HttpClient httpClient,
            ILocalStorageService localStorageService,
            NavigationManager navigationManager,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
            _webHostEnvironment = webHostEnvironment;
            pathToFakeData = $"{_navigationManager.BaseUri}fake-data.json";
        }

        public async Task Add(ItemModel model)
        {
            // Get the current data
            var currentData = await _localStorageService.GetItemAsync<List<Item>>("data");

            // Simulate the Id
            model.Id = currentData.Max(item => item.Id) + 1;

            // Add the item to the current data
            currentData.Add(ItemFactory.Create(model));

            

            // Save the data
            await _localStorageService.SetItemAsync("data", currentData);
        }

        public async Task<int> Count()
        {
            if (await _localStorageService.GetItemAsync<Item[]>("data") == null)
            {
                var originalData =
                    await _httpClient
                    .GetFromJsonAsync<Item[]>(pathToFakeData);

                await _localStorageService.SetItemAsync("data", originalData);
            }

            return (await _localStorageService.GetItemAsync<Item[]>("data")).Length;
        }

        public async Task Delete(int id)
        {
            var currentData = await _localStorageService.GetItemAsync<List<Item>>("data");
            var item = currentData.FirstOrDefault(w => w.Id == id);
            currentData.Remove(item);

            await _localStorageService.SetItemAsync("data", currentData);
        }

        public async Task<Item> GetById(int id)
        {
            var item = (await _localStorageService.GetItemAsync<List<Item>>("data")).FirstOrDefault(w => w.Id == id);

            if (item == null)
            {
                throw new FileNotFoundException($"could not find item #{id}");
            }

            return item;
        }

        public Task<List<CraftingRecipe>> GetRecipes()
        {
            var items = new List<CraftingRecipe>
            {
                new CraftingRecipe
                {
                    Give = new Item { DisplayName = "Diamond", Name = "diamond" },
                    Have = new List<List<string>>
                    {
                        new List<string> { "dirt", "dirt", "dirt" },
                        new List<string> { "dirt", null, "dirt" },
                        new List<string> { "dirt", "dirt", "dirt" }
                    }
                }
            };

            return Task.FromResult(items);
        }

        public async Task<List<Item>> List(int currentPage, int pageSize)
        {
            if (await _localStorageService.GetItemAsync<Item[]>("data") == null)
            {
                var originalData =
                    await _httpClient
                    .GetFromJsonAsync<Item[]>(pathToFakeData);
                await _localStorageService.SetItemAsync("data", originalData);
            }

            return (await _localStorageService.GetItemAsync<Item[]>("data")).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task Update(int id, ItemModel model)
        {
            var currentData = await _localStorageService.GetItemAsync<List<Item>>("data");
            var item = currentData.FirstOrDefault(w => w.Id == id);
            if (item == null)
            {
                throw new Exception($"Unable to found the item with ID: {id}");
            }


            ItemFactory.Update(item, model);

            await _localStorageService.SetItemAsync("data", currentData);
        }
    }
}
