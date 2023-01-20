using blazor_lab.Models;
using blazor_lab.Services;
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
        public IDataService DataService { get; set; }

        [Inject]
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // Do not treat this action if is not the first render
            if (!firstRender)
            {
                return;
            }

            var currentData = await DataService.List(1, 10);
        }

        private async Task OnReadData(DataGridReadDataEventArgs<Item> e)
        {
            if (e.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (!e.CancellationToken.IsCancellationRequested)
            {
                items = await DataService.List(e.Page, e.PageSize);
                totalItems = await DataService.Count();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            items = await DataService.List(1, 10);
        }
    }
}
