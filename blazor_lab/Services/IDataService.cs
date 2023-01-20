using blazor_lab.Models;

namespace blazor_lab.Services
{
    public interface IDataService
    {
        Task Add(ItemModel itemModel);
        Task<int> Count();
        Task<List<Item>> List(int currentPage, int pageSize);
    }
}
