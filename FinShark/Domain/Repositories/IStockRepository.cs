using Domain.DTOs.Stock;
using Domain.Helpers;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IStockRepository
    {
        Task<IQueryable<Stock>> GetAllAsync();

        Task<Stock?> GetByIdAsync(int id);

        Task<Stock?> GetBySymbolAsync(string symbol);

        Task<Stock> CreateAsync(Stock newStock);

        Task<Stock?> UpdateAsync(Stock stock);

        Task<Stock?> DeleteAsync(Stock stock);

        Task<bool> StockExistsAsync(int id);
    }
}
