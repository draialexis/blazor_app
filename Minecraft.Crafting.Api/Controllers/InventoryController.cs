// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryController.cs" company="UCA Clermont-Ferrand">
//     Copyright (c) UCA Clermont-Ferrand All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Minecraft.Crafting.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Minecraft.Crafting.Api.Models;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// The inventory controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        /// <summary>
        /// The json serializer options.
        /// </summary>
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        /// <summary>
        /// Adds to inventory.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The async task.</returns>
        [HttpPost]
        [Route("")]
        public Task AddToInventory(InventoryModel item)
        {
            var data = JsonSerializer.Deserialize<List<InventoryModel>>(System.IO.File.ReadAllText("Data/inventory.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the inventory.");
            }

            data.Add(item);

            System.IO.File.WriteAllText("Data/inventory.json", JsonSerializer.Serialize(data, _jsonSerializerOptions));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes from inventory.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The async task.</returns>
        [HttpDelete]
        [Route("")]
        public Task DeleteFromInventory(InventoryModel item)
        {
            if (!System.IO.File.Exists("Data/inventory.json"))
            {
                throw new Exception($"Unable to found the item with name: {item.ItemName}");
            }

            var data = JsonSerializer.Deserialize<List<InventoryModel>>(System.IO.File.ReadAllText("Data/inventory.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the inventory.");
            }

            var inventoryItem = data.FirstOrDefault(w => w.ItemName == item.ItemName && w.Position == item.Position);

            if (inventoryItem == null)
            {
                throw new Exception($"Unable to found the item with name: {item.ItemName} at position: {item.Position}");
            }

            data.Remove(inventoryItem);

            System.IO.File.WriteAllText("Data/inventory.json", JsonSerializer.Serialize(data, _jsonSerializerOptions));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the inventory.
        /// </summary>
        /// <returns>The inventory.</returns>
        [HttpGet]
        [Route("")]
        public Task<List<InventoryModel>> GetInventory()
        {
            if (!System.IO.File.Exists("Data/inventory.json"))
            {
                return Task.FromResult(new List<InventoryModel>());
            }

            var data = JsonSerializer.Deserialize<List<InventoryModel>>(System.IO.File.ReadAllText("Data/inventory.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the inventory.");
            }

            return Task.FromResult(data);
        }

        /// <summary>
        /// Updates the inventory.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The async task.</returns>
        [HttpPut]
        [Route("")]
        public Task UpdateInventory(InventoryModel item)
        {
            var data = JsonSerializer.Deserialize<List<InventoryModel>>(System.IO.File.ReadAllText("Data/inventory.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the inventory.");
            }

            var inventoryItem = data.FirstOrDefault(w => w.ItemName == item.ItemName && w.Position == item.Position);

            if (inventoryItem == null)
            {
                throw new Exception($"Unable to found the item with name: {item.ItemName} at position: {item.Position}");
            }

            inventoryItem.ItemName = item.ItemName;
            inventoryItem.Position = item.Position;

            System.IO.File.WriteAllText("Data/inventory.json", JsonSerializer.Serialize(data, _jsonSerializerOptions));

            return Task.CompletedTask;
        }
    }
}