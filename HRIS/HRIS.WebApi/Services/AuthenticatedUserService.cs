using HRIS.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HRIS.WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
            BranchId = httpContextAccessor.HttpContext?.User?.FindFirstValue("branchId");
        }

        public string UserId { get; }
        public string BranchId { get; }
    }
}
