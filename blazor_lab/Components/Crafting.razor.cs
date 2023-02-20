using blazor_lab.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace blazor_lab.Components
{
    public partial class Crafting
    {
        private Item _recipeResult;

        public Crafting()
        {
            Actions = new ObservableCollection<CraftingAction>();
            Actions.CollectionChanged += OnActionsCollectionChanged;
            this.RecipeItems = new List<Item> { null, null, null, null, null, null, null, null, null };
        }

        public ObservableCollection<CraftingAction> Actions { get; set; }
        public Item CurrentDragItem { get; set; }

        [Parameter]
        public List<Item> Items { get; set; }

        public List<Item> RecipeItems { get; set; }

        public Item RecipeResult
        {
            get => this._recipeResult;
            set
            {
                if (this._recipeResult == value)
                {
                    return;
                }

                this._recipeResult = value;
                this.StateHasChanged();
            }
        }

        [Parameter]
        public List<CraftingRecipe> Recipes { get; set; }

        /// <summary>
        /// Gets or sets the java script runtime.
        /// </summary>
        [Inject]
        internal IJSRuntime JavaScriptRuntime { get; set; }

        public void CheckRecipe()
        {
            RecipeResult = null;

            // Get the current model
            var currentModel = string.Join("|", this.RecipeItems.Select(s => s != null ? s.Name : string.Empty));

            this.Actions.Add(new CraftingAction { Action = $"Items : {currentModel}" });

            foreach (var craftingRecipe in Recipes)
            {
                // Get the recipe model
                var recipeModel = string.Join("|", craftingRecipe.Have.SelectMany(s => s));

                this.Actions.Add(new CraftingAction { Action = $"Recipe model : {recipeModel}" });

                if (currentModel == recipeModel)
                {
                    RecipeResult = craftingRecipe.Give;
                }
            }
        }

        private void OnActionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            JavaScriptRuntime.InvokeVoidAsync("Crafting.AddActions", e.NewItems);
        }
    }
}
