using Domain.Models;

namespace Application.Interfaces
{
    public interface IPortfolioService
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> CreatePortfolio(AppUser appUser, Stock stock);
        Task<Portfolio> DeletePortfolio(AppUser appUser, Stock stock);
        Task<bool> PortfolioExists(AppUser appUser, Stock stock);
    }
}
