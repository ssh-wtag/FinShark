using Domain.Models;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
