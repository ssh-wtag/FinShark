using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _context;

        public StockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock> CreateAsync(Stock newStock)
        {
            await _context.Stocks.AddAsync(newStock);
            await _context.SaveChangesAsync();
            return newStock;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO updateStockRequestDTO)
        {
            var oldStock = await _context.Stocks.FindAsync(id);

            if (oldStock == null)
                return null;

            oldStock.Symbol = updateStockRequestDTO.Symbol;
            oldStock.CompanyName = updateStockRequestDTO.CompanyName;
            oldStock.Purchase = updateStockRequestDTO.Purchase;
            oldStock.LastDiv = updateStockRequestDTO.LastDiv;
            oldStock.Industry = updateStockRequestDTO.Industry;
            oldStock.MarketCap = updateStockRequestDTO.MarketCap;

            await _context.SaveChangesAsync();
            return oldStock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingStock == null)
                return null;

            _context.Stocks.Remove(existingStock);
            await _context.SaveChangesAsync();

            return existingStock;
        }
    }
}
