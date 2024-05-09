using InternetBanking.Core.Application.Dtos.Account;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task SignOutAsync();
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin);
        Task<List<AuthenticationResponse>> GetAllUsersAsync();
        Task<AuthenticationResponse> GetUserByIdAsync(string id);
        Task<string> UpdateUserAsync(RegisterRequest request, string userId);
        Task<string> ActivateUserAsync(string id);
        Task<string> InactivateUserAsync(string id);
    }
}
