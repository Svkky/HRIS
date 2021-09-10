using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.DTOs.Menu;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "GlobalAdmin,StoreAdmin,BranchAdmin")]
    public class ProcessMenuToUsersController : ControllerBase
    {
        private readonly IAssignMenuToUserService Service;
        public ProcessMenuToUsersController(IAssignMenuToUserService Service)
        {
            this.Service = Service;
        }

        [HttpPost]
        [Route("AssignMenu")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> AssignMenu([FromBody] MenuRequest request)
        {
            if (!(request.menus.Count() > 0))
            {
                return Ok(ResponseHelper.FailureMessage("You haven't checked any pages yet!"));
            }
            var response = await Service.CreateMenuRequest(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Menu was successfully assign to users"));
            return Ok(ResponseHelper.FailureMessage("Failure Assigning menu to users"));
        }

        [HttpPut("UpdateAssignMenu")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult UpdateAssignMenu([FromBody] MenuRequest request)
        {
            var response = Service.UpdateRequest(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("updating Assign Menu was successfull"));
            return Ok(ResponseHelper.FailureMessage("Failure updating Assign menu to users"));
        }

        [HttpPost("RemoveAssignMenu")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult RemoveAssignMenu([FromBody] MenuRequest request)
        {
            if (!(request.menus.Count() > 0))
            {
                return Ok(ResponseHelper.FailureMessage("You haven't checked any pages yet!"));
            }
            var response = Service.DeleteMenuRequest(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Vat was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting Assigned menu to users"));
        }


    }
}
