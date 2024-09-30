using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[controller]")] // [Route("api/stock")]
    [ApiController]

    public class StockController : ControllerBase
    {
        #region Initialization

        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        #endregion

        #region Implementation

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockRepository.GetAllAsync();
            var stocksDTO = stocks.Select(s => s.ToStockDTOFromStock());

            return Ok(stocksDTO);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);

            if(stock == null)
                return NotFound();

            return Ok(stock.ToStockDTOFromStock());
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO createDTO)
        {
            var newStock = createDTO.ToStockFromCreateStockRequestDTO();

            await _stockRepository.CreateAsync(newStock);

            return CreatedAtAction(nameof(GetById), new { id = newStock.Id}, newStock.ToStockDTOFromStock() );
        }



        [HttpPut] // [HttpPut("{id}")]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
        {
            var updatedStock = await _stockRepository.UpdateAsync(id, updateDTO);

            if(updatedStock == null)
                return NotFound();

            return Ok(updatedStock.ToStockDTOFromStock());
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletedStock = await _stockRepository.DeleteAsync(id);

            if (deletedStock == null)
                return NotFound();

            return NoContent();
        }

        #endregion
    }
}
