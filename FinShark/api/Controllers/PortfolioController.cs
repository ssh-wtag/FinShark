using api.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        #region Initialization

        private readonly UserManager<AppUser> _userManager;

        private readonly IStockService _stockService;

        private readonly IPortfolioService _portfolioService;

        public PortfolioController(UserManager<AppUser> userManager, IStockService stockService, IPortfolioService portfolioService)
        {
            _userManager = userManager;
            _stockService = stockService;
            _portfolioService = portfolioService;
        }

        #endregion



        #region Implementation

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio =  await _portfolioService.GetUserPortfolio(appUser);

            return Ok(userPortfolio);
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockService.GetBySymbolAsync(symbol);

            if (stock == null)
                return BadRequest("Stock Not Found");
            
            if(await _portfolioService.PortfolioExists(appUser, stock))
                return BadRequest("Cannot Add Same Stock to Portfolio");

            var portfolioModel = await _portfolioService.CreatePortfolio(appUser, stock);
                
            if (portfolioModel == null)
                return StatusCode(500, "Could Not Create");
            else
                return Created();
        }



        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockService.GetBySymbolAsync(symbol);

            if(stock == null)
                return BadRequest("Stock Not Found");

            if (await _portfolioService.PortfolioExists(appUser, stock))
            {
                await _portfolioService.DeletePortfolio(appUser, stock);
                return Ok();
            }
            else
            {
                return BadRequest("Stock Not In Your Portfolio");
            }
        }

        #endregion
    }
}
