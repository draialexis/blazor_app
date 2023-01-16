using blazor_lab.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace blazor_lab.Pages
{
    public partial class Add
    {

        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        [Inject]
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        /// <summary>
        /// The default enchant categories.
        /// </summary>
        private List<string> enchantCategories = new() { "armor", "armor_head", "armor_chest", "weapon", "digger", "breakable", "vanishable" };

        /// <summary>
        /// The default repair with.
        /// </summary>
        private List<string> repairWith = new() { "oak_planks", "spruce_planks", "birch_planks", "jungle_planks", "acacia_planks", "dark_oak_planks", "crimson_planks", "warped_planks" };

        /// <summary>
        /// The current item model
        /// </summary>
        private ItemModel itemModel = new()
        {
            EnchantCategories = new List<string>(),
            RepairWith = new List<string>()
        };


        private async void HandleValidSubmit()
        {
            // Get the current data
            var currentData = await LocalStorageService.GetItemAsync<List<Item>>("data");

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
            var imagePathInfo = new DirectoryInfo($"{WebHostEnvironment.WebRootPath}/images");

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
            await LocalStorageService.SetItemAsync("data", currentData);
        }
        private async Task LoadImage(InputFileChangeEventArgs e)
        {
            // Set the model's image to the image saved on file
            using (var memoryStream = new MemoryStream())
            {
                await e.File.OpenReadStream().CopyToAsync(memoryStream);
                itemModel.ImageContent = memoryStream.ToArray();
            }
        }

        private void OnEnchantCategoriesChange(string item, object checkedValue)
        {
            if ((bool)checkedValue)
            {
                if (!itemModel.EnchantCategories.Contains(item))
                {
                    itemModel.EnchantCategories.Add(item);
                }

                return;
            }

            if (itemModel.EnchantCategories.Contains(item))
            {
                itemModel.EnchantCategories.Remove(item);
            }
        }

        private void OnRepairWithChange(string item, object checkedValue)
        {
            if ((bool)checkedValue)
            {
                if (!itemModel.RepairWith.Contains(item))
                {
                    itemModel.RepairWith.Add(item);
                }

                return;
            }

            if (itemModel.RepairWith.Contains(item))
            {
                itemModel.RepairWith.Remove(item);
            }
        }
    }
}
