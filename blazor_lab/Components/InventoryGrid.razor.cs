using Microsoft.AspNetCore.Components;
using Minecraft.Crafting.Api.Models;


namespace blazor_lab.Components
{

    public partial class InventoryGrid
    {
        [Parameter]
        public List<InventoryModel> Inventory { get; set; }

        /// <summary>
        /// Used by GetItemImageBase64 in this component, rather than calling our DataService every time.
        /// A very basic cache, not kept up to date in any way, but event listeners could be set up in the future
        /// </summary>
        [Parameter]
        public List<Models.Item> Items { get; set; }

        public string GetItemImageBase64(string displayName)
        {
            var item = Items.FirstOrDefault(i => i.DisplayName == displayName);
            return item?.ImageBase64;
        }
    }
}
