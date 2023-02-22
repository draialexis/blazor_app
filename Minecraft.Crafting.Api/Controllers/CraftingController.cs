// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CraftingController.cs" company="UCA Clermont-Ferrand">
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
    /// The crafting controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CraftingController : ControllerBase
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
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The async task.</returns>
        [HttpPost]
        [Route("")]
        public Task Add(Item item)
        {
            var data = JsonSerializer.Deserialize<List<Item>>(System.IO.File.ReadAllText("Data/items.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the items.");
            }

            // Simulate the Id
            item.Id = data.Max(s => s.Id) + 1;

            data.Add(item);

            System.IO.File.WriteAllText("Data/items.json", JsonSerializer.Serialize(data, _jsonSerializerOptions));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <returns>All items.</returns>
        [HttpGet]
        [Route("all")]
        public Task<List<Item>> All()
        {
            var data = JsonSerializer.Deserialize<List<Item>>(System.IO.File.ReadAllText("Data/items.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the items.");
            }

            return Task.FromResult(data.ToList());
        }

        /// <summary>
        /// Count the number of items.
        /// </summary>
        /// <returns>The number of items.</returns>
        [HttpGet]
        [Route("count")]
        public Task<int> Count()
        {
            var data = JsonSerializer.Deserialize<List<Item>>(System.IO.File.ReadAllText("Data/items.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the items.");
            }

            return Task.FromResult(data.Count);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The async task.</returns>
        [HttpDelete]
        [Route("{id}")]
        public Task Delete(int id)
        {
            var data = JsonSerializer.Deserialize<List<Item>>(System.IO.File.ReadAllText("Data/items.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the items.");
            }

            var item = data.FirstOrDefault(w => w.Id == id);

            if (item == null)
            {
                throw new Exception($"Unable to found the item with ID: {id}");
            }

            data.Remove(item);

            System.IO.File.WriteAllText("Data/items.json", JsonSerializer.Serialize(data, _jsonSerializerOptions));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the item by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The item.</returns>
        [HttpGet]
        [Route("{id}")]
        public Task<Item> GetById(int id)
        {
            var data = JsonSerializer.Deserialize<List<Item>>(System.IO.File.ReadAllText("Data/items.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the items.");
            }

            var item = data.FirstOrDefault(w => w.Id == id);

            if (item == null)
            {
                throw new Exception($"Unable to found the item with ID: {id}");
            }

            return Task.FromResult(item);
        }

        /// <summary>
        /// Gets the item by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// The item.
        /// </returns>
        [HttpGet]
        [Route("by-name/{name}")]
        public Task<Item> GetByName(string name)
        {
            var data = JsonSerializer.Deserialize<List<Item>>(System.IO.File.ReadAllText("Data/items.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the items.");
            }

            var item = data.FirstOrDefault(w => w.Name.ToLowerInvariant() == name.ToLowerInvariant());

            if (item == null)
            {
                throw new Exception($"Unable to found the item with name: {name}");
            }

            return Task.FromResult(item);
        }

        /// <summary>
        /// Gets the recipes.
        /// </summary>
        /// <returns>The recipes.</returns>
        [HttpGet]
        [Route("recipe")]
        public Task<List<Recipe>> GetRecipe()
        {
            if (!System.IO.File.Exists("Data/convert-recipes.json"))
            {
                ResetRecipes();
            }

            var data = JsonSerializer.Deserialize<List<Recipe>>(System.IO.File.ReadAllText("Data/convert-recipes.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the recipes.");
            }

            return Task.FromResult(data);
        }

        /// <summary>
        /// Get the items with pagination.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>The items.</returns>
        [HttpGet]
        [Route("")]
        public Task<List<Item>> List(int currentPage, int pageSize)
        {
            var data = JsonSerializer.Deserialize<List<Item>>(System.IO.File.ReadAllText("Data/items.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the items.");
            }

            return Task.FromResult(data.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// Resets the items.
        /// </summary>
        /// <returns>The async task.</returns>
        [HttpGet]
        [Route("reset-items")]
        public Task ResetItems()
        {
            if (!System.IO.File.Exists("Data/items.json"))
            {
                System.IO.File.Delete("Data/items.json");
            }

            var data = JsonSerializer.Deserialize<List<Item>>(System.IO.File.ReadAllText("Data/items-original.json"), _jsonSerializerOptions);

            if (data == null)
            {
                throw new Exception("Unable to get the items.");
            }

            var defaultImage = Convert.ToBase64String(System.IO.File.ReadAllBytes("Images/default.png"));

            var imageTranslation = new Dictionary<string, string>
            {
                { "stone_slab", "smooth_stone_slab_side" },
                { "sticky_piston", "piston_top_sticky" },
                { "mob_spawner", "spawner" },
                { "chest", "chest_minecart" },
                { "stone_stairs", "stairs" },
            };

            foreach (var item in data)
            {
                var imageFilepath = defaultImage;

                if (System.IO.File.Exists($"Images/{item.Name}.png"))
                {
                    imageFilepath = Convert.ToBase64String(System.IO.File.ReadAllBytes($"Images/{item.Name}.png"));
                }

                if (imageFilepath == defaultImage && System.IO.File.Exists($"Images/{item.Name}_top.png"))
                {
                    imageFilepath = Convert.ToBase64String(System.IO.File.ReadAllBytes($"Images/{item.Name}_top.png"));
                }

                if (imageFilepath == defaultImage && System.IO.File.Exists($"Images/{item.Name}_front.png"))
                {
                    imageFilepath = Convert.ToBase64String(System.IO.File.ReadAllBytes($"Images/{item.Name}_front.png"));
                }

                if (imageFilepath == defaultImage && System.IO.File.Exists($"Images/white_{item.Name}.png"))
                {
                    imageFilepath = Convert.ToBase64String(System.IO.File.ReadAllBytes($"Images/white_{item.Name}.png"));
                }

                if (imageFilepath == defaultImage && System.IO.File.Exists($"Images/oak_{item.Name}.png"))
                {
                    imageFilepath = Convert.ToBase64String(System.IO.File.ReadAllBytes($"Images/oak_{item.Name}.png"));
                }

                if (imageFilepath == defaultImage && System.IO.File.Exists($"Images/{item.DisplayName.ToLower().Replace(" ", "_")}.png"))
                {
                    imageFilepath = Convert.ToBase64String(System.IO.File.ReadAllBytes($"Images/{item.DisplayName.ToLower().Replace(" ", "_")}.png"));
                }

                if (imageFilepath == defaultImage && imageTranslation.ContainsKey(item.Name))
                {
                    imageFilepath = Convert.ToBase64String(System.IO.File.ReadAllBytes($"Images/{imageTranslation[item.Name]}.png"));
                }

                item.ImageBase64 = imageFilepath;
            }

            System.IO.File.WriteAllText("Data/items.json", JsonSerializer.Serialize(data, _jsonSerializerOptions));

            return Task.FromResult(data);
        }

        /// <summary>
        /// Resets the recipes.
        /// </summary>
        /// <returns>The async task.</returns>
        [HttpGet]
        [Route("reset-recipes")]
        public Task ResetRecipes()
        {
            if (!System.IO.File.Exists("Data/convert-recipes.json"))
            {
                System.IO.File.Delete("Data/convert-recipes.json");
            }

            ConvertRecipes();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="item">The item.</param>
        /// <returns>The async task.</returns>
        [HttpPut]
        [Route("{id}")]
        public Task Update(int id, Item item)
        {
            var data = JsonSerializer.Deserialize<List<Item>>(System.IO.File.ReadAllText("Data/items.json"), _jsonSerializerOptions);

            var itemOriginal = data?.FirstOrDefault(w => w.Id == id);

            if (itemOriginal == null)
            {
                throw new Exception($"Unable to found the item with ID: {id}");
            }

            itemOriginal.Id = item.Id;
            itemOriginal.Name = item.Name;
            itemOriginal.CreatedDate = item.CreatedDate;
            itemOriginal.DisplayName = item.DisplayName;
            itemOriginal.EnchantCategories = item.EnchantCategories;
            itemOriginal.MaxDurability = item.MaxDurability;
            itemOriginal.RepairWith = item.RepairWith;
            itemOriginal.StackSize = item.StackSize;
            itemOriginal.UpdatedDate = item.UpdatedDate;
            itemOriginal.ImageBase64 = item.ImageBase64;

            System.IO.File.WriteAllText("Data/items.json", JsonSerializer.Serialize(data, _jsonSerializerOptions));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="inShape">The in shape.</param>
        /// <param name="line">The line.</param>
        /// <param name="row">The row.</param>
        /// <returns>The name of the item.</returns>
        private static string GetItemName(List<Item> items, InShape[][] inShape, int line, int row)
        {
            if (inShape.Length < line + 1)
            {
                return null;
            }

            if (inShape[line].Length < row + 1)
            {
                return null;
            }

            var id = inShape[line][row].Integer ?? inShape[line][row].IngredientClass?.Id;

            if (id == null)
            {
                return null;
            }

            return GetItemName(items, id.Value);
        }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>The name of the item.</returns>
        private static string GetItemName(List<Item> items, long id)
        {
            var item = items.FirstOrDefault(w => w.Id == id);
            return item?.Name;
        }

        /// <summary>
        /// Converts the recipes.
        /// </summary>
        private void ConvertRecipes()
        {
            var data = JsonSerializer.Deserialize<List<Item>>(System.IO.File.ReadAllText("Data/items.json"), _jsonSerializerOptions);

            if (data == null)
            {
                return;
            }

            var recipes = Recipes.FromJson(System.IO.File.ReadAllText("Data/recipes.json"));

            var items = new List<Recipe>();

            foreach (var recipe in recipes.SelectMany(s => s.Value))
            {
                if (recipe.InShape == null)
                {
                    continue;
                }

                var giveItem = data.FirstOrDefault(w => w.Id == recipe.Result.Id);

                if (giveItem == null)
                {
                    continue;
                }

                items.Add(new Recipe
                {
                    Give = giveItem,
                    Have = new List<List<string>>
                    {
                        new()
                        {
                            GetItemName(data, recipe.InShape, 0, 0),
                            GetItemName(data, recipe.InShape, 0, 1),
                            GetItemName(data, recipe.InShape, 0, 2)
                        },
                        new()
                        {
                            GetItemName(data, recipe.InShape, 1, 0),
                            GetItemName(data, recipe.InShape, 1, 1),
                            GetItemName(data, recipe.InShape, 1, 2)
                        },
                        new()
                        {
                            GetItemName(data, recipe.InShape, 2, 0),
                            GetItemName(data, recipe.InShape, 2, 1),
                            GetItemName(data, recipe.InShape, 2, 2)
                        }
                    }
                });
            }

            System.IO.File.WriteAllText("Data/convert-recipes.json", JsonSerializer.Serialize(items, _jsonSerializerOptions));
        }
    }
}