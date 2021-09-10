using HRIS.Application.DTOs.Dashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IDashBoardRepository
    {
        string GetAllDashboardValues();
        string GetAllDashboardValues1();
        string GetAllDashboardValues2();
    }
}