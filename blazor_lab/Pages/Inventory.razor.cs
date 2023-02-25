using blazor_lab.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Minecraft.Crafting.Api.Models;
using System.Diagnostics;

namespace blazor_lab.Pages
{
    public partial class Inventory
    {
        private List<Models.Item> Items = new();

        [Inject]
        public IStringLocalizer<Inventory> Localizer { get; set; }

        [Inject]
        private DataApiService DataApiService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Items = await DataApiService.All();
        }

        private List<InventoryModel> FreshInventory = Enumerable.Range(1, 18).Select(_ => new InventoryModel()).ToList();
    }
}
