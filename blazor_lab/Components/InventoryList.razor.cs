using blazor_lab.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace blazor_lab.Components
{
    public partial class InventoryList
    {
        [Inject]
        public IStringLocalizer<InventoryList> Localizer { get; set; }

        [Parameter]
        public List<Item> Items { get; set; }

        private List<Item> _filteredItems;

        private string searchQuery = "";

        private int currentPage = 1;
        private int pageSize = 10;

        private void UpdateFilteredItems()
        {
            _filteredItems = string.IsNullOrEmpty(searchQuery) ? Items : Items.Where(i => i.DisplayName.ToLower().Contains(searchQuery.ToLower())).ToList();
            VisibleItems = _filteredItems.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        private List<Item> _visibleItems;

        private List<Item> VisibleItems
        {
            get => _visibleItems;
            set
            {
                _visibleItems = value;
                StateHasChanged();
            }
        }

        private int TotalPages => (int)Math.Ceiling((double)_filteredItems.Count / pageSize);

        private int TotalItems => _filteredItems.Count;

        private void GoToPage(int page)
        {
            currentPage = page;
            UpdateFilteredItems();
        }

        protected override void OnParametersSet()
        {
            UpdateFilteredItems();
        }

        private async Task OnInputChange(ChangeEventArgs e)
        {
            searchQuery = e.Value.ToString();
            await Task.Delay(250); // debounce the search to avoid excessive API requests
            UpdateFilteredItems();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                UpdateFilteredItems();
            }
        }
    }

}
