using HRIS.Application.DTOs.Account;
using HRIS.Application.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.Application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code, string password);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<Response<string>> Logout();
        Task<int> ResendEmail(string email, string webUrl);
        Task<List<UserDTO>> GetUsers(string loggedInUserId);
        Task<Response<string>> RemoveUser(string userId);

        Task<Response<List<RolesDTO>>> GetRoles();

        Task<int> Enable(string userId);
        Task<int> Disable(string userId);
    }
}
