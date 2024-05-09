using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace InternetBanking.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.UserName}";
                return response;
            }

            if (user.IsActive != true)
            {
                response.HasError = true;
                response.Error = $"Your account is currently inactive. Please contact the administrator for assistance.";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials for {request.UserName}";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsActive = user.IsActive;

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"username '{request.UserName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                IdNumber = request.IdNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                IsActive = true
            };

            if (!Enum.TryParse(typeof(Roles), request.Role, out var userRole))
            {
                response.HasError = true;
                response.Error = $"Invalid user role";
                return response;
            }

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                response.Id = user.Id;
                await _userManager.AddToRoleAsync(user, userRole.ToString());
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred trying to register the user.";
                return response;
            }

            return response;
        }

        public async Task<string> UpdateUserAsync(RegisterRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "User not found.";
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.IdNumber = request.IdNumber;
            user.UserName = request.UserName;

            if (request.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, token, request.Password);
                if (!resetResult.Succeeded)
                {
                    return $"An error occurred while trying to reset the password.";
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return $"Update successful.";
            }
            else
            {
                return $"An error ocurred trying to update the user.";
            }
        }

        public async Task<string> ActivateUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return "User not found";
            }

            user.IsActive = true;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return $"Successful activation.";
            }
            else
            {
                return $"An error ocurred trying to activate the user.";
            }
        }

        public async Task<string> InactivateUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return "User not found";
            }

            user.IsActive = false;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return $"Successful inactivation.";
            }
            else
            {
                return $"An error ocurred trying to inactivate the user.";
            }
        }

        public async Task<AuthenticationResponse> GetUserByIdAsync(string id)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                response.HasError = true;
                response.Error = "User not found";
                return response;
            }
            var roles = await _userManager.GetRolesAsync(user);

            response.Id = user.Id;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Email = user.Email;
            response.IdNumber = user.IdNumber;
            response.UserName = user.UserName;
            response.Roles = roles.ToList();
            response.IsActive = user.IsActive;

            return response;
        }

        public async Task<List<AuthenticationResponse>> GetAllUsersAsync()
        {
            var userList = await _userManager.Users.ToListAsync();

            List<AuthenticationResponse> userResponses = new();

            foreach (var user in userList)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var authenticationResponse = new AuthenticationResponse
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    IdNumber = user.IdNumber,
                    UserName = user.UserName,
                    Roles = roles.ToList(),
                    IsActive = user.IsActive
                };

                userResponses.Add(authenticationResponse);
            }

            return userResponses;
        }

    }
}
