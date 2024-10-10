using Application.Interfaces;
using Domain.Models;
using Domain.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }



        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _portfolioRepository.GetUserPortfolio(user);
        }



        public async Task<Portfolio> CreatePortfolio(AppUser appUser, Stock stock)
        {
            var userPortfolio = await GetUserPortfolio(appUser);

            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockID = stock.Id,
            };

            return await _portfolioRepository.CreatePortfolio(portfolioModel);
        }



        public async Task<Portfolio> DeletePortfolio(AppUser appUser, Stock stock)
        {
            return await _portfolioRepository.DeletePortfolio(appUser, stock);
        }



        public async Task<bool> PortfolioExists(AppUser appUser, Stock stock)
        {
            var userPortfolio = await GetUserPortfolio(appUser);

            if (userPortfolio.Any(e => e.Symbol == stock.Symbol))
                return true;

            return false;
        }
    }
}
