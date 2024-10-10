using Infrastructure.Data;
using Domain.DTOs.Stock;
using Domain.Helpers;
using Domain.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        #region Initialization

        private readonly AppDbContext _context;

        public StockRepository(AppDbContext context)
        {
            _context = context;
        }

        #endregion



        #region Implementation

        public async Task<IQueryable<Stock>> GetAllAsync()
        {
            return _context.Stocks.Include(s => s.Comments).ThenInclude(s => s.AppUser);
        }



        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(s => s.Comments).ThenInclude(s => s.AppUser).FirstOrDefaultAsync(x => x.Id == id);
        }



        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(x => x.Symbol == symbol);
        }



        public async Task<Stock> CreateAsync(Stock newStock)
        {
            await _context.Stocks.AddAsync(newStock);
            await _context.SaveChangesAsync();
            return newStock;
        }



        public async Task<Stock?> UpdateAsync(Stock stock)
        {
            await _context.SaveChangesAsync();
            return stock;
        }



        public async Task<Stock?> DeleteAsync(Stock stock)
        {
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return stock;
        }



        public async Task<bool> StockExistsAsync(int id)
        {
            return await _context.Stocks.AnyAsync(x => x.Id == id);
        }

        #endregion
    }
}
