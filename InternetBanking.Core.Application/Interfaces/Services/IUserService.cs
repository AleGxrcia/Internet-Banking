using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.ViewModels.User;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task ActiveUser(string id);
        Task<List<UserViewModel>> GetAllAsync();
        Task<SaveUserViewModel> GetByIdAsync(string id);
        Task InactiveUser(string id);
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task SignOutAsync();
        Task UpdateAsync(SaveUserViewModel vm, string id);
    }
}
