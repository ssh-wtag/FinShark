using api.DTOs.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();

        Task<Stock?> GetByIdAsync(int id);

        Task<Stock> CreateAsync(Stock newStock);

        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO updateStockRequestDTO);

        Task<Stock?> DeleteAsync(int id);
    }
}
