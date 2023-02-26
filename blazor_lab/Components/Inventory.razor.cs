using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Minecraft.Crafting.Api.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Item = blazor_lab.Models.Item;

namespace blazor_lab.Components
{
    public partial class Inventory
    {
        [Inject]
        public IStringLocalizer<Inventory> Localizer { get; set; }
        [Inject]
        internal IJSRuntime JavaScriptRuntime { get; set; }
        [Parameter]
        public List<Item> Items { get; set; }
        public InventoryModel? CurrentDragItem { get; set; } = new();
        public List<InventoryModel> InventoryContent { get; set; } = Enumerable.Range(1, 18).Select(_ => new InventoryModel()).ToList();

        public Inventory()
        {
            Actions = new ObservableCollection<InventoryAction>();
            Actions.CollectionChanged += OnActionsCollectionChanged;
        }

        public ObservableCollection<InventoryAction> Actions { get; set; }

        private void OnActionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            JavaScriptRuntime.InvokeVoidAsync("Inventory.AddActions", e.NewItems);
        }
    }
}
