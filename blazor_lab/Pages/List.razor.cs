using blazor_lab.Models;
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
        public NavigationManager NavigationManager { get; set; }

        private async Task OnReadData(DataGridReadDataEventArgs<Item> e)
        {
            if (e.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            //real API =>
            //var response = await Http.GetJsonAsync<Item[]>( $"http://my-api/api/data?page={e.Page}&pageSize={e.PageSize}" );
            var response =
                (
                    await HttpClient.GetFromJsonAsync<Item[]>(
                        $"{NavigationManager.BaseUri}fake-data.json"
                    )
                )
                .Skip((e.Page - 1) * e.PageSize)
                .Take(e.PageSize)
                .ToList();

            if (!e.CancellationToken.IsCancellationRequested)
            {
                totalItems =
                    (
                        await HttpClient.GetFromJsonAsync<List<Item>>(
                            $"{NavigationManager.BaseUri}fake-data.json"
                        )
                    )
                    .Count;

                items = new List<Item>(response);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            items = await HttpClient.GetFromJsonAsync<List<Item>>($"{NavigationManager.BaseUri}fake-data.json");
        }
    }
}
