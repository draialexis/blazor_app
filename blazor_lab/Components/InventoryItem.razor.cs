using blazor_lab.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Minecraft.Crafting.Api;
using Minecraft.Crafting.Api.Models;
using System.Diagnostics;
using Item = blazor_lab.Models.Item;

namespace blazor_lab.Components
{
    public partial class InventoryItem
    {
        [Parameter]
        public Item? Item { get; set; }

        [Parameter]
        public int Position { get; set; } = -1;

        [Parameter]
        public bool IsInList { get; set; }

        [Parameter]
        public bool IsInInventory { get; set; }

        [CascadingParameter]
        public InventoryList? ListParent { get; set; }

        [CascadingParameter]
        public Inventory? InventoryParent { get; set; }

        public InventoryModel InventoryModel { get; set; } = new InventoryModel();

        public InventoryItem()
        {
            if (Item is not null && IsInList)
            {
                InventoryModel.ItemName = Item.DisplayName;
                InventoryModel.NumberItem = 1;
            }
        }

        internal void OnDrop()
        {
            if (IsInList)
            {
                return;
            }

            if (IsInInventory)
            {
                if (InventoryModel.Position == -1) // new inventoryModel
                {
                    InventoryModel = InventoryParent!.CurrentDragItem!;
                    InventoryModel.Position = Position;
                    InventoryParent.InventoryContent.Insert(Position, InventoryModel);
                }
                else
                {
                    if (InventoryModel.ItemName == InventoryParent!.CurrentDragItem!.ItemName) // adding to an existing stack
                    {
                        InventoryModel.NumberItem += 1;
                    }
                }
            }
            StateHasChanged();
        }

        internal void OnDragEnd()
        {
            if (IsInList)
            {
                ListParent!.Parent.CurrentDragItem = null;
            }
            else if (IsInInventory)
            {
                InventoryParent!.CurrentDragItem = null;
            }
        }

        private void OnDragStart()
        {
            if (InventoryModel is not null)
            {
                if (IsInList)
                {
                    ListParent!.Parent.CurrentDragItem = InventoryModel;
                }
                else if (IsInInventory)
                {
                    InventoryParent!.CurrentDragItem = InventoryModel;
                }
            }
        }
    }
}
