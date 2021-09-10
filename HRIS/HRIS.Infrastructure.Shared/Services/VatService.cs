using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Vat;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Domain.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HRIS.Infrastructure.Shared.Services
{
    public class VatService : IVatService
    {
        private readonly ILogger<VatService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        //  private readonly IOptions<ConnectionStrings> _connectionString;
        private string constring;
        IOptions<ConnectionStrings> myconnectionString;
        private readonly IDapper _dapper;

        public VatService(ILogger<VatService> logger,
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
        public int CreateVat(CreateVatDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("Description", model.Name);
                param.Add("Percentage", model.percentage);
                param.Add("CreatedBy", _authenticatedUser.UserId);
                param.Add("BranchId", model.branchId);
                var response = _dapper.Execute(ApplicationConstant.SP_vat, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving vat");
                return 0;
            }
        }

        public int DeleteVat(DeleteVatDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.DELETE);
                param.Add("VatID", model.vatId);
                param.Add("DeletedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.SP_vat, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting vat");
                return 0;
            }
        }

        public List<VatDTO> GetAllVat(int branchId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                param.Add("BranchId", branchId);
                var response = _dapper.GetAll<VatDTO>(ApplicationConstant.SP_vat, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while returning vat");
                return null;
            }
        }

        public VatDTO GetVatById(int VatId,int branchId)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("VatID", VatId);
                param.Add("BranchId", branchId);
                var response = _dapper.Get<VatDTO>(ApplicationConstant.SP_vat, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting a single vat");
                return new VatDTO();
            }
        }

        public int UpdateVat(UpdateVatDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.UPDATE);
                param.Add("VatID", model.vatId);
                param.Add("Description", model.Name);
                param.Add("percentage", model.percentage);
                param.Add("UpdatedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.SP_vat, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a vat");
                return 0;
            }
        }
    }
}
