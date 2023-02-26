using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Minecraft.Crafting.Api.Models;
using Item = blazor_lab.Models.Item;

namespace blazor_lab.Components
{
    public partial class Inventory
    {
        [Inject]
        public IStringLocalizer<Inventory> Localizer { get; set; }
        [Parameter]
        public List<Item> Items { get; set; }
        public InventoryModel? CurrentDragItem { get; set; } = new();
        public List<InventoryModel> InventoryContent { get; set; } = Enumerable.Range(1, 18).Select(_ => new InventoryModel()).ToList();
    }
}
