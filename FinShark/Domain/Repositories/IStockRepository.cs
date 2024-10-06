using Domain.DTOs.Stock;
using Domain.Helpers;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(StockQueryObject query);

        Task<Stock?> GetByIdAsync(int id);

        Task<Stock?> GetBySymbolAsync(string symbol);

        Task<Stock> CreateAsync(Stock newStock);

        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO updateStockRequestDTO);

        Task<Stock?> DeleteAsync(int id);

        Task<bool> StockExists(int id);
    }
}
