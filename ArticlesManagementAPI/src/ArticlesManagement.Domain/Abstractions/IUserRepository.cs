
using ArticlesManagement.Domain.Entities;
using System.Threading.Tasks;

namespace ArticlesManagement.Domain.Abstractions
{

    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<User> GetById(int userId);
        Task<User> AddUser(User user);
        Task DeleteUser(User user);
        Task StoreVerificationCode(int userId, string code);
        Task<bool> VerifyUser(int userId, string code);
    }
}
