using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
            var stocks = _context.Stocks
                .ToList()
                .Select(s => s.ToStockDTOFromStock());

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);

            if(stock == null)
                return NotFound();

            return Ok(stock.ToStockDTOFromStock());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDTO createDTO)
        {
            var newStock = createDTO.ToStockFromCreateStockRequestDTO();
            _context.Stocks.Add(newStock);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newStock.Id}, newStock.ToStockDTOFromStock() );
        }

        [HttpPut] // [HttpPut("{id}")]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
        {
            var oldStock = _context.Stocks.FirstOrDefault(x => x.Id == id);

            if(oldStock == null)
                return NotFound();

            oldStock.Symbol = updateDTO.Symbol;
            oldStock.CompanyName = updateDTO.CompanyName;
            oldStock.Purchase = updateDTO.Purchase;
            oldStock.LastDiv = updateDTO.LastDiv;
            oldStock.Industry = updateDTO.Industry;
            oldStock.MarketCap = updateDTO.MarketCap;

            _context.SaveChanges();

            return Ok(oldStock.ToStockDTOFromStock());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);

            if(stock == null)
                return NotFound();

            _context.Stocks.Remove(stock);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
