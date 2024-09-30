using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[controller]")] // [Route("api/stock")]
    [ApiController]

    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StockController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.Stocks.ToListAsync();
            var stocksDTO = stocks.Select(s => s.ToStockDTOFromStock());

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id);

            if(stock == null)
                return NotFound();

            return Ok(stock.ToStockDTOFromStock());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO createDTO)
        {
            var newStock = createDTO.ToStockFromCreateStockRequestDTO();

            await _context.Stocks.AddAsync(newStock);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newStock.Id}, newStock.ToStockDTOFromStock() );
        }

        [HttpPut] // [HttpPut("{id}")]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
        {
            var oldStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if(oldStock == null)
                return NotFound();

            oldStock.Symbol = updateDTO.Symbol;
            oldStock.CompanyName = updateDTO.CompanyName;
            oldStock.Purchase = updateDTO.Purchase;
            oldStock.LastDiv = updateDTO.LastDiv;
            oldStock.Industry = updateDTO.Industry;
            oldStock.MarketCap = updateDTO.MarketCap;

            await _context.SaveChangesAsync();

            return Ok(oldStock.ToStockDTOFromStock());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id);

            if(stock == null)
                return NotFound();

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
