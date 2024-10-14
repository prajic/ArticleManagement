using ArticlesManagement.Domain.Abstractions;
using ArticlesManagement.Domain.Entities;
using ArticlesManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace ArticlesManagement.Infrastructure.Repositories
{

    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetById(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Author)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task<User> AddUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user; 
        }

        public async Task DeleteUser(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task StoreVerificationCode(int userId, string code)
        {
            var verificationCode = new VerificationCode
            {
                UserId = userId,
                Code = code,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            _dbContext.VerificationCodes.Add(verificationCode);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> VerifyUser(int userId, string code)
        {
            var verificationCode = await _dbContext.VerificationCodes
                .FirstOrDefaultAsync(vc => vc.UserId == userId && vc.Code == code);

            if (verificationCode == null || verificationCode.IsUsed || verificationCode.ExpiresAt < DateTime.UtcNow)
            {
                return false; 
            }

            verificationCode.IsUsed = true;

            _dbContext.VerificationCodes.Remove(verificationCode);

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

            user.IsVerified = true;

            _dbContext.Update(user);

            await _dbContext.SaveChangesAsync();
            return true; 
        }

    }
}
