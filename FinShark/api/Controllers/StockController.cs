using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAll([FromQuery] StockQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stocks = await _stockRepository.GetAllAsync(query);
            var stocksDTO = stocks.Select(s => s.ToStockDTOFromStock()).ToList();

            return Ok(stocksDTO);
        }



        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);

            if(stock == null)
                return NotFound();

            return Ok(stock.ToStockDTOFromStock());
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO createDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var newStock = createDTO.ToStockFromCreateStockRequestDTO();

            await _stockRepository.CreateAsync(newStock);

            return CreatedAtAction(nameof(GetById), new { id = newStock.Id}, newStock.ToStockDTOFromStock() );
        }



        [HttpPut] // [HttpPut("{id}")]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedStock = await _stockRepository.UpdateAsync(id, updateDTO);

            if(updatedStock == null)
                return NotFound();

            return Ok(updatedStock.ToStockDTOFromStock());
        }



        [HttpDelete("{id:int}")]
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
