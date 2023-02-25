using blazor_lab.Services;
using Microsoft.AspNetCore.Components;
using Minecraft.Crafting.Api.Models;


namespace blazor_lab.Components
{

    public partial class InventoryGrid
    {
        [Parameter]
        public List<InventoryModel> Inventory { get; set; }

        public List<Models.Item> Items { get; set; } = new List<Models.Item>();

        [Inject]
        public IConfiguration Config { get; set; }


        [Inject]
        private DataApiService dataApiService { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Items = await dataApiService.All();
        }

        public string GetItemImageBase64(string displayName)
        {
            var item = Items.FirstOrDefault(i => i.DisplayName == displayName);
            return item?.ImageBase64;
        }
    }
}
