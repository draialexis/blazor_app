using blazor_lab.Models;
using blazor_lab.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace blazor_lab.Pages
{
    public partial class Add
    {

        [Inject]
        public IDataService DataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }    

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
            await DataService.Add(itemModel);
            NavigationManager.NavigateTo("list");
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
