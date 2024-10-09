using Application.Interfaces;
using Application.Mappers;
using Domain.DTOs.Stock;
using Domain.Helpers;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;


        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }


        public async Task<List<Stock>> GetAllAsync(StockQueryObject query)
        {
            var stocks = await _stockRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(query.Symbol))
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    if (query.IsDescending)
                        stocks = stocks.OrderByDescending(s => s.Symbol);
                    else
                        stocks = stocks.OrderBy(s => s.Symbol);
                }
                else if (query.SortBy.Equals("Company Name", StringComparison.OrdinalIgnoreCase))
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
            return await _stockRepository.GetByIdAsync(id);
        }


        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _stockRepository.GetBySymbolAsync(symbol);
        }


        public async Task<Stock> CreateAsync(CreateStockRequestDTO createDTO)
        {
            var newStock = createDTO.ToStockFromCreateStockRequestDTO();

            return await _stockRepository.CreateAsync(newStock);
        }


        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO updateStockRequestDTO)
        {
            var oldStock = await _stockRepository.GetByIdAsync(id);

            if (oldStock == null)
                return null;

            oldStock.Symbol = updateStockRequestDTO.Symbol;
            oldStock.CompanyName = updateStockRequestDTO.CompanyName;
            oldStock.Purchase = updateStockRequestDTO.Purchase;
            oldStock.LastDiv = updateStockRequestDTO.LastDiv;
            oldStock.Industry = updateStockRequestDTO.Industry;
            oldStock.MarketCap = updateStockRequestDTO.MarketCap;

            return await _stockRepository.UpdateAsync(oldStock);
        }


        public async Task<Stock?> DeleteAsync(int id)
        {
            Stock? existingStock = await _stockRepository.GetByIdAsync(id);

            if (existingStock == null)
                return null;

            return await _stockRepository.DeleteAsync(existingStock);
        }

        
        public async Task<bool> StockExistsAsync(int id)
        {
            return await _stockRepository.StockExistsAsync(id);
        }
    }
}
