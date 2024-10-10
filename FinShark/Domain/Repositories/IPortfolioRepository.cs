using Domain.Models;

namespace Domain.Repositories
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);

        Task<Portfolio> CreatePortfolio(Portfolio portfolio);

        Task<Portfolio> DeletePortfolio(AppUser appUser, Stock stock);
    }
}
