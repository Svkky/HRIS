using System.Threading.Tasks;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IAuditRepository
    {
        Task CreateAudit(string userId, string action);
    }
}
