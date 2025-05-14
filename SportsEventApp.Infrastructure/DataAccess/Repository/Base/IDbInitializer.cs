
namespace SportsEventApp.Infrastructure.DataAccess.Repository.Base
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }
}