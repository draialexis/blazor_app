﻿using blazor_lab.Factories;
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

        public async Task Add(ItemModel model)
        {
            // Get the current data
            var currentData = await _localStorageService.GetItemAsync<List<Item>>("data");

            // Simulate the Id
            model.Id = currentData.Max(item => item.Id) + 1;

            // Add the item to the current data
            currentData.Add(ItemFactory.Create(model));

            // Save the image
            var imagePathInfo = new DirectoryInfo($"{_webHostEnvironment.WebRootPath}/images");

            // Check if the folder "images" exist
            if (!imagePathInfo.Exists)
            {
                imagePathInfo.Create();
            }

            // Determine the image name
            var fileName = new FileInfo($"{imagePathInfo}/{model.Name}.png");

            // Write the file content
            await File.WriteAllBytesAsync(fileName.FullName, model.ImageContent);

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

        public async Task<Item> GetById(int id)
        {
            var item = (await _localStorageService.GetItemAsync<List<Item>>("data")).FirstOrDefault(w => w.Id == id);

            if (item == null)
            {
                throw new FileNotFoundException($"could not find item #{id}");
            }

            return item;
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
            var item = await GetById(id);
            var imagePathInfo = new DirectoryInfo($"{_webHostEnvironment.WebRootPath}/images");
            if (!imagePathInfo.Exists)
            {
                imagePathInfo.Create();
            }
            if (item.Name != model.Name)
            {
                var oldFileName = new FileInfo($"{imagePathInfo}/{item.Name}.png");
                if (oldFileName.Exists)
                {
                    File.Delete(oldFileName.FullName);
                }
            }
            var fileName = new FileInfo($"{imagePathInfo}/{model.Name}.png");
            await File.WriteAllBytesAsync(fileName.FullName, model.ImageContent);

            ItemFactory.Update(item, model);

            await _localStorageService.SetItemAsync("data", currentData);  
        }
    }
}
