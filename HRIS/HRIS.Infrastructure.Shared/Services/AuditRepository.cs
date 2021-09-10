using HRIS.Application.Interfaces.Repositories;
using HRIS.Domain.Entities;
using HRIS.Infrastructure.Identity.Models;
using HRIS.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Shared.Services
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuditRepository(ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task CreateAudit(string userId, string action)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var newAudit = new Audit
            {
                Action = action,
                UserID = userId,
                UserFullName = user.FirstName + " " + user.LastName,
                Date = DateTime.Now
            };
            await _context.Audit.AddAsync(newAudit);
            await _context.SaveChangesAsync();
        }
    }
}
