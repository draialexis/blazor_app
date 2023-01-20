using blazor_lab.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;

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

        public async Task Add(ItemModel itemModel)
        {
            // Get the current data
            var currentData = await _localStorageService.GetItemAsync<List<Item>>("data");

            // Simulate the Id
            itemModel.Id = currentData.Max(item => item.Id) + 1;

            // Add the item to the current data
            currentData.Add(new Item
            {
                Id = itemModel.Id,
                DisplayName = itemModel.DisplayName,
                Name = itemModel.Name,
                RepairWith = itemModel.RepairWith,
                EnchantCategories = itemModel.EnchantCategories,
                MaxDurability = itemModel.MaxDurability,
                StackSize = itemModel.StackSize,
                CreatedDate = DateTime.Now
            });

            // Save the image
            var imagePathInfo = new DirectoryInfo($"{_webHostEnvironment.WebRootPath}/images");

            // Check if the folder "images" exist
            if (!imagePathInfo.Exists)
            {
                imagePathInfo.Create();
            }

            // Determine the image name
            var fileName = new FileInfo($"{imagePathInfo}/{itemModel.Name}.png");

            // Write the file content
            await File.WriteAllBytesAsync(fileName.FullName, itemModel.ImageContent);

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
    }
}
