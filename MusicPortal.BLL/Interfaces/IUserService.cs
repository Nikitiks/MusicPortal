using MusicPortal.Common.Entities;
using MusicPortal.Common.Models;

namespace MusicPortal.BLL.Interfaces
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(LoginModel model);
        Task<bool> RegisterAsync(RegisterModel model);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ToggleUserStatusAsync(int id);
        Task<bool> UserExistsAsync(string username, string email);
    }
}
