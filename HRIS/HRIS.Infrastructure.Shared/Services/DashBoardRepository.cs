using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Dashboard;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Domain.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Data;

namespace HRIS.Infrastructure.Shared.Services
{
    public class DashBoardRepository : IDashBoardRepository
    {
        private readonly ILogger<DashBoardRepository> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private string constring;
        IOptions<ConnectionStrings> myconnectionString;
        private readonly IDapper _dapper;
        string TotalPaid;
        string Vat;
        string Orders;
        public DashBoardRepository(ILogger<DashBoardRepository> logger,
                             IAuthenticatedUserService authenticatedUser,
                             IOptions<ConnectionStrings> connectionString,
                             IDapper dapper)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            myconnectionString = connectionString;
            _dapper = dapper;
            constring = myconnectionString.Value.DefaultConnection;
        }

        public string GetAllDashboardValues()
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", 1);
                param.Add("BranchId", _authenticatedUser.BranchId);
                var response = _dapper.GetAll<DashBoardDTO>(ApplicationConstant.Sp_Dashboard, param, commandType: CommandType.StoredProcedure);

                foreach (var item in response)
                {
                    TotalPaid = item.TotalPaid;

                }
                return TotalPaid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating  category");
                return null;
            }
        }

        public string GetAllDashboardValues1()
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", 2);
                param.Add("BranchId", _authenticatedUser.BranchId);
                var response = _dapper.GetAll<DashBoardDTO>(ApplicationConstant.Sp_Dashboard, param, commandType: CommandType.StoredProcedure);
                foreach (var item in response)
                {
                    Vat = item.TotalVat;

                }
                return Vat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating  category");
                return null;
            }
        }

        public string GetAllDashboardValues2()
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", 3);
                param.Add("BranchId", _authenticatedUser.BranchId);
                var response = _dapper.GetAll<DashBoardDTO>(ApplicationConstant.Sp_Dashboard, param, commandType: CommandType.StoredProcedure);
                foreach (var item in response)
                {
                    Orders = item.Orders;

                }
                return Orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating  category");
                return null;
            }
        }





    }
}
