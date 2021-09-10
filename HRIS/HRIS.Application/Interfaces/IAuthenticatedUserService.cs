namespace HRIS.Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
        string BranchId { get; }
    }
}
