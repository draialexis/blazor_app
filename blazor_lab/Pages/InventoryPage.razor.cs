using blazor_lab.Components;
using blazor_lab.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace blazor_lab.Pages
{
    public partial class InventoryPage
    {
        [Inject]
        public IStringLocalizer<InventoryPage> Localizer { get; set; }

        [Inject]
        private IDataService DataService { get; set; }


        private List<Models.Item> Items = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRenderAsync(firstRender);

            if (!firstRender)
            {
                return;
            }

            Items = await DataService.List(0, await DataService.Count());

            StateHasChanged();
        }
    }
}
