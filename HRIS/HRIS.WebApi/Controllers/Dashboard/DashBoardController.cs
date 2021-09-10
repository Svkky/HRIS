using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Dashboard;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.WebApi.Controllers.Dashboard
{
    [ApiVersion("1.0")]
    [Authorize]
    public class DashBoardController : BaseApiController
    {
        private readonly IDashBoardRepository _DashboadRepository;

        public DashBoardController(IDashBoardRepository DashboadRepository)
        {
            _DashboadRepository = DashboadRepository;

        }

        [HttpGet]
        [Route("GetAllDashboardValues")]
        public IActionResult GetAllDashboardValues()
        {
            var TotalPaid = _DashboadRepository.GetAllDashboardValues();
            var Vat = _DashboadRepository.GetAllDashboardValues1();
            var NoOfOrder = _DashboadRepository.GetAllDashboardValues2();
            List<DashBoardDTO> Payload = new List<DashBoardDTO>();
            Payload.Add(new DashBoardDTO
            {
                 Orders= NoOfOrder,
                  TotalPaid= TotalPaid,
                   TotalVat= Vat

            });
            if (TotalPaid != null && Vat!=null && NoOfOrder!=null)
               
                return Ok(new Response<List<DashBoardDTO>>
                {
                    //Data = TotalPaid,
                     
                    
                   Data= Payload,
                   Message = "list was retrieved successfully",
                   ResponseCode = ApplicationConstant.SuccessResponseCode,
                   StatusCode = ApplicationConstant.SuccessStatusCode,
                   Succeeded = true

                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving All Dashboard Values"));
        }
    }
}
