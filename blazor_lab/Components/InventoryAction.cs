using Minecraft.Crafting.Api.Models;

namespace blazor_lab.Components
{
    public class InventoryAction
    {
        public string? Action { get; set; }
        public int Position { get; set; }
        public InventoryModel? InventoryModel { get; set; }
    }
}
