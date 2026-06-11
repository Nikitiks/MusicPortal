using MusicPortal.BLL.Interfaces;
using MusicPortal.Common.Entities;
using MusicPortal.Common.Models;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> AuthenticateAsync(LoginModel model)
        {
            if (model.Username == null || model.Password == null)
                return null;

            var user = await _unitOfWork.Users.GetByUsernameAsync(model.Username);
            
            if (user == null || user.Password != model.Password)
                return null;

            if (!user.IsActive)
                return null;

            return user;
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            if (await UserExistsAsync(model.Username!, model.Email!))
                return false;

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                IsActive = true,
                IsAdmin = false,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _unitOfWork.Users.GetByUsernameAsync(username);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.Users.GetAllAsync();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                return false;

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleUserStatusAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                return false;

            user.IsActive = !user.IsActive;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            var userByUsername = await _unitOfWork.Users.GetByUsernameAsync(username);
            var userByEmail = await _unitOfWork.Users.GetByEmailAsync(email);

            return userByUsername != null || userByEmail != null;
        }
    }
}
