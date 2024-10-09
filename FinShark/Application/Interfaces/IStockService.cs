using Domain.DTOs.Stock;
using Domain.Helpers;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IStockService
    {
        Task<List<Stock>> GetAllAsync(StockQueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<Stock> CreateAsync(CreateStockRequestDTO createDTO);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO updateStockRequestDTO);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> StockExistsAsync(int id);
    }
}
