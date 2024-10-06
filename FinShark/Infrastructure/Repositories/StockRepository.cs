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
        private readonly AppDbContext _context;

        public StockRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Stock>> GetAllAsync(StockQueryObject query)
        {
            var stocks = _context.Stocks.Include(s => s.Comments).ThenInclude(s => s.AppUser).AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Symbol))
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    if (query.IsDescending)
                        stocks = stocks.OrderByDescending(s => s.Symbol);
                    else
                        stocks = stocks.OrderBy(s => s.Symbol);
                }
                else if(query.SortBy.Equals("Company Name", StringComparison.OrdinalIgnoreCase))
                {
                    if (query.IsDescending)
                        stocks = stocks.OrderByDescending(s => s.CompanyName);
                    else
                        stocks = stocks.OrderBy(s => s.CompanyName);
                }
            }

            int skipNum = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNum).Take(query.PageSize).ToListAsync();
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


        public async Task<bool> StockExists(int id)
        {
            return await _context.Stocks.AnyAsync(x => x.Id == id);
        }
    }
}
