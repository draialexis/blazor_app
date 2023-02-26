using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Minecraft.Crafting.Api.Models;
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

        public InventoryModel? InventoryModel { get; set; } = new InventoryModel();

        public InventoryItem()
        {
            if (IsInInventory)
            {
                InventoryModel.ImageBase64 = null;
                InventoryModel.ItemName = "";
                InventoryModel.NumberItem = 0;
                InventoryModel.Position = Position;
            }
        }

        internal void OnDrop()
        {
            if (IsInList)
            {
                ListParent!.Parent.CurrentDragItem = null;
                return;
            }

            if (IsInInventory)
            {
                InventoryModel ??= new();
                if (InventoryModel.ItemName.IsNullOrEmpty()) // new inventoryModel
                {
                    InventoryModel.ImageBase64 = InventoryParent!.CurrentDragItem!.ImageBase64;
                    InventoryModel.ItemName = InventoryParent!.CurrentDragItem!.ItemName;
                    InventoryModel.Position = Position;
                    InventoryModel.NumberItem = 1;
                    InventoryParent.InventoryContent.Insert(Position, InventoryModel);
                }
                else
                {
                    if (InventoryModel.ItemName == InventoryParent!.CurrentDragItem!.ItemName) // adding to an existing stack
                    {
                        InventoryModel.NumberItem += 1;
                    }
                }
                InventoryParent!.CurrentDragItem = null;
            }
        }

        private void OnDragStart()
        {
            if (IsInList)
            {
                ListParent!.Parent.CurrentDragItem = new InventoryModel
                {
                    ImageBase64 = Item!.ImageBase64,
                    ItemName = Item!.DisplayName,
                    NumberItem = 1,
                    Position = -1
                };
            }
            else if (IsInInventory) // delete item stack if it is dragged from inventory
            {
                InventoryModel = new InventoryModel
                {
                    ImageBase64 = null,
                    ItemName = "",
                    NumberItem = 0,
                    Position = Position
                };
                InventoryParent!.CurrentDragItem = null;
            }
        }
    }
}
