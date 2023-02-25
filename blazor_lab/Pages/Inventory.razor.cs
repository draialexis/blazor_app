using Minecraft.Crafting.Api.Models;

namespace blazor_lab.Pages
{
    public partial class Inventory
    {
        private List<InventoryModel> Stuff = Enumerable.Range(1, 18).Select(_ => new InventoryModel()).ToList();
    }
}
