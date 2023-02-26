using blazor_lab.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace blazor_lab.Components
{
    public partial class Inventory
    {
        [Inject]
        public IStringLocalizer<Inventory> Localizer { get; set; }

        [Parameter]
        public List<Item> Items { get; set; } = new();
    }
}
