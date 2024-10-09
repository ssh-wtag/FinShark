using Domain.DTOs.Stock;
using Domain.Helpers;
using Domain.Repositories;
using Application.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;

namespace api.Controllers
{
    [Route("api/[controller]")] // [Route("api/stock")]
    [ApiController]

    public class StockController : ControllerBase
    {
        #region Initialization

        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        #endregion

        #region Implementation

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] StockQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stocks = await _stockService.GetAllAsync(query);
            var stocksDTO = stocks.Select(s => s.ToStockDTOFromStock()).ToList();

            return Ok(stocksDTO);
        }



        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockService.GetByIdAsync(id);

            if(stock == null)
                return NotFound();

            return Ok(stock.ToStockDTOFromStock());
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO createDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var newStock = await _stockService.CreateAsync(createDTO);

            return CreatedAtAction(nameof(GetById), new { id = newStock.Id}, newStock.ToStockDTOFromStock() );
        }



        [HttpPut] // [HttpPut("{id}")]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedStock = await _stockService.UpdateAsync(id, updateDTO);

            if(updatedStock == null)
                return NotFound();

            return Ok(updatedStock.ToStockDTOFromStock());
        }



        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletedStock = await _stockService.DeleteAsync(id);

            if (deletedStock == null)
                return NotFound();

            return NoContent();
        }

        #endregion
    }
}
