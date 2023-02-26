using blazor_lab.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace blazor_lab.Components
{
    public enum SortOption
    {
        NameDescending,
        NameAscending
    }
    public partial class InventoryList
    {
        [Inject]
        public IStringLocalizer<InventoryList> Localizer { get; set; }

        [Parameter]
        public List<Item> Items { get; set; }

        private List<Item> _filteredItems;

        private string searchQuery = "";

        private int currentPage = 1;
        private readonly int pageSize = 10;

        private void UpdateFilteredItems()
        {
            _filteredItems = string.IsNullOrEmpty(searchQuery)
                ? Items
                : Items.Where(i => i.DisplayName.ToLower().Contains(searchQuery.ToLower())).ToList();
            SortItems();
            VisibleItems = _filteredItems.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        private void SortItems()
        {
            switch (_sortOption)
            {
                case SortOption.NameAscending:
                    _filteredItems = _filteredItems.OrderBy(i => i.DisplayName).ToList();
                    break;
                case SortOption.NameDescending:
                    _filteredItems = _filteredItems.OrderByDescending(i => i.DisplayName).ToList();
                    break;
            }
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

        private SortOption _sortOption = SortOption.NameDescending;

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
            currentPage = 1; // Go back to page 1 when user is searching
            UpdateFilteredItems();
        }
        private void OnSortOptionChanged(ChangeEventArgs e)
        {
            if (Enum.TryParse(e.Value.ToString(), out SortOption sortOption))
            {
                _sortOption = sortOption;
                UpdateFilteredItems();
            }
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
