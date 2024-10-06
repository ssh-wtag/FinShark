using api.DTOs.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces
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
