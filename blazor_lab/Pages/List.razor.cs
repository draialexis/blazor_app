using blazor_lab.Models;
using Blazored.LocalStorage;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;

namespace blazor_lab.Pages
{
    public partial class List
    {
        private List<Item> items;

        private int totalItems;

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // Do not treat this action if is not the first render
            if (!firstRender)
            {
                return;
            }

            var currentData = await LocalStorageService.GetItemAsync<Item[]>("data");

            // Check if data exist in the local storage
            if (currentData == null)
            {
                // this code add in the local storage the fake data (we load the data sync for initialize the data before load the OnReadData method)
                var originalData = HttpClient.GetFromJsonAsync<Item[]>($"{NavigationManager.BaseUri}fake-data.json").Result;
                await LocalStorageService.SetItemAsync("data", originalData);
            }
        }

        private async Task OnReadData(DataGridReadDataEventArgs<Item> e)
        {
            if (e.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            //Real API =>
            //var response = await HttpClient.GetJsonAsync<Data[]>( $"http://my-api/api/data?page={e.Page}&pageSize={e.PageSize}" );
            var response = (await LocalStorageService.GetItemAsync<Item[]>("data")).Skip((e.Page - 1) * e.PageSize).Take(e.PageSize).ToList();

            if (!e.CancellationToken.IsCancellationRequested)
            {
                totalItems = (await LocalStorageService.GetItemAsync<List<Item>>("data")).Count;
                items = new List<Item>(response); // an actual data for the current page
            }
        }

        protected override async Task OnInitializedAsync()
        {
            items = await HttpClient.GetFromJsonAsync<List<Item>>($"{NavigationManager.BaseUri}fake-data.json");
        }
    }
}
