using Microsoft.AspNetCore.Components;
using Minecraft.Crafting.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace blazor_lab.Components
{

    public partial class InventoryGrid
    {
        [Parameter]
        public List<InventoryModel> Inventory { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public IConfiguration Config { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Items = await HttpClient.GetFromJsonAsync<List<Item>>($"{Config["CraftingApi:BaseUrl"]}/api/Crafting/all");
        }

        public string GetItemImageBase64(string displayName)
        {
            var item = Items.FirstOrDefault(i => i.DisplayName == displayName);
            return item?.ImageBase64;
        }
    }
}
