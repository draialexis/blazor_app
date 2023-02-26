# Blazor Inventory Management App
This is a sample Blazor app for managing inventory items. It allows you to drag and drop items from an inventory list to an inventory grid.

## Getting Started
To run the app, you'll need to have .NET 6.0 installed. You can download it from the [official .NET website](https://dotnet.microsoft.com/download/dotnet/6.0).

In Visual Studio, right-click the `blazor_lab` solution, and click on `Properties`. 

In the `property pages`, set both projects to `Start`.

Then you can actually start the app by clicking on the `Start` button next to a drop-down menu that should say `<Multiple Starup Projects>`.

Two browser windows will open. You can ignore the *Swagger* window, and in the *blazor_lab* window, you can use the navbar to go straight to `Inventory`. 

## App Structure
The app consists of several components:

### `InventoryPage`
This is the main page component that displays the inventory. It retrieves the inventory items from an `IDataService` and passes them to the `Inventory` component.

### `Inventory`
This component manages the inventory grid and the inventory actions. It uses `InventoryItem` to display each inventory slot, and `InventoryList` to display the list of items on the right.

### `InventoryItem`
This component represents an inventory slot, and can display either an `Item` or an `InventoryModel`. Both represent an "item", but the latter is a more minimalist version, and knows its position in an inventory. 

`InventoryItem` allows the user to drag items from the inventory list and drop them into the inventory grid.

### `InventoryList`
This component displays the list of items in the inventory, and allows the user to search and sort them. It uses InventoryItem to display each item.

## Usage
When you first load the app, you will be able to navigate to `Inventory` and see the inventory grid and the inventory list side by side. The inventory list displays all the available items, and the inventory grid displays the items that have been added to the inventory.

To add an item to the inventory, simply drag an item from the inventory list and drop it onto an empty slot in the inventory grid.

To remove an item from the inventory, drag the item from the inventory grid.

To search for an item, type the search query in the search box at the top of the inventory list. The list will update to display only the items that match the search query.

To sort the items, select a sort option from the dropdown list at the top of the inventory list. The list will update to display the items in the selected order.