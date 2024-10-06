using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> CreatePortfolio(Portfolio portfolio);

        Task<Portfolio> DeletePortfolio(AppUser appUser, String symbol);
    }
}
