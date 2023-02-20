using blazor_lab.Components;
using blazor_lab.Models;

namespace blazor_lab.Services
{
    public interface IDataService
    {
        Task Add(ItemModel itemModel);
        Task<int> Count();
        Task<List<Item>> List(int currentPage, int pageSize);
        Task<Item> GetById(int id);
        Task Update(int id, ItemModel model);
        Task Delete(int id);
        Task<List<CraftingRecipe>> GetRecipes();
    }
}
