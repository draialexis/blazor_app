using Blazorise;
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

        public InventoryModel InventoryModel { get; set; } = new InventoryModel();

        public InventoryItem()
        {
            if (IsInInventory)
            {
                InitInventoryModel();
            }
        }

        internal void OnDrop()
        {
            if (IsInList)
            {
                ListParent!.Parent.CurrentDragItem = null;
                ListParent.Parent.Actions.Add(new InventoryAction
                {
                    Action = "Tried to drop on list",
                    InventoryModel = null,
                    Position = -1
                });
                return;
            }

            if (IsInInventory && InventoryParent!.CurrentDragItem is not null)
            {
                if (InventoryModel!.ItemName.IsNullOrEmpty()) // new inventoryModel
                {
                    InventoryModel.ImageBase64 = InventoryParent!.CurrentDragItem!.ImageBase64;
                    InventoryModel.ItemName = InventoryParent!.CurrentDragItem!.ItemName;
                    InventoryModel.Position = Position;
                    InventoryModel.NumberItem = 1;
                    InventoryParent.InventoryContent.Insert(Position, InventoryModel);
                    InventoryParent.Actions.Add(new InventoryAction
                    {
                        Action = "Drop on inventory -- new -- successful",
                        InventoryModel = InventoryModel,
                        Position = Position
                    });
                }
                else if (InventoryModel.ItemName == InventoryParent!.CurrentDragItem!.ItemName) // adding to an existing stack
                {
                    InventoryModel.NumberItem += 1;
                    InventoryParent.Actions.Add(new InventoryAction
                    {
                        Action = "Drop on inventory -- add -- successful",
                        InventoryModel = InventoryModel,
                        Position = Position
                    });
                }
                else
                {
                    InventoryParent.Actions.Add(new InventoryAction
                    {
                        Action = "Drop on inventory -- unsuccessful",
                        InventoryModel = null,
                        Position = Position
                    });
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
                ListParent.Parent.Actions.Add(new InventoryAction
                {
                    Action = "Drag from list",
                    InventoryModel = ListParent.Parent.CurrentDragItem,
                    Position = ListParent.Parent.CurrentDragItem.Position
                });
            }
            else if (IsInInventory)
            // delete item stack if it is dragged from inventory
            {
                InventoryParent!.Actions.Add(new InventoryAction
                {
                    Action = "Drag from inventory (deleting)",
                    InventoryModel = InventoryParent.CurrentDragItem,
                    Position = Position
                });
                InventoryParent.CurrentDragItem = null;
                InitInventoryModel();
            }
        }

        private void InitInventoryModel()
        {
            InventoryModel.ImageBase64 = null;
            InventoryModel.ItemName = "";
            InventoryModel.NumberItem = 0;
            InventoryModel.Position = Position;
        }
    }
}
