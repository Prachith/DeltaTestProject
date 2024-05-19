using CommonData;
using CommonData.Models;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class UserService
    {
        private readonly DataContext _dbContext;

        public UserService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddUser(User user)
        {
            _dbContext.Users.Add(new Users
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Password = user.Password,
                Email = user.Email,
                IsDeleted = user.IsDeleted,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
            });
            await _dbContext.SaveChangesAsync();
            return true;
        }
        
        public async Task<Users?> GetUser(User user)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email || x.UserId == user.UserId);
        }

        public async Task<Users?> GetUserByUserId(string userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<List<Users>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> Login(User loginUser)
        {
            var user = await GetUser(loginUser);

            if (user == null)  return new User();

            if (user.Password == loginUser.Password && (user.Email == loginUser.Email || user.UserId == loginUser.UserId))
            {
                return new User()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Password = user.Password,
                    Email = user.Email,
                    UserId = user.UserId,
                };
            }
            else
            {
                return new User();
            }
        }
    }
}
