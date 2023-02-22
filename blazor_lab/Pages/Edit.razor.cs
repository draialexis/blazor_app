using blazor_lab.Factories;
using blazor_lab.Models;
using blazor_lab.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace blazor_lab.Pages
{
    public partial class Edit
    {
        [Parameter]
        public int Id { get; set; }

        /// <summary>
        /// The default enchant categories.
        /// </summary>
        private readonly List<string> enchantCategories = new() { "armor", "armor_head", "armor_chest", "weapon", "digger", "breakable", "vanishable" };

        /// <summary>
        /// The current item model
        /// </summary>
        private ItemModel itemModel = new()
        {
            EnchantCategories = new List<string>(),
            RepairWith = new List<string>()
        };

        /// <summary>
        /// The default repair with.
        /// </summary>
        private readonly List<string> repairWith = new() { "oak_planks", "spruce_planks", "birch_planks", "jungle_planks", "acacia_planks", "dark_oak_planks", "crimson_planks", "warped_planks" };

        [Inject]
        public IDataService DataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var item = await DataService.GetById(Id);

            var fileContent = await File.ReadAllBytesAsync($"{WebHostEnvironment.WebRootPath}/images/default.png");

            // Set the model with the item
            itemModel = ItemFactory.ToModel(item, fileContent);
        }

        private async Task HandleValidSubmit()
        {
            await DataService.Update(Id, itemModel);

            NavigationManager.NavigateTo("list");
        }

        private async Task LoadImage(InputFileChangeEventArgs e)
        {
            // Set the content of the image to the model
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
