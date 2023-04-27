using InventoryMg.BLL.DTOs;
using InventoryMg.BLL.DTOs.Request;
using InventoryMg.BLL.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.VisualBasic;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryMg.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
           _userService = userService;
        }

        [HttpGet("user-profile")]
        [SwaggerOperation(Summary = "Get profile details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Return UserProfile")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
        public async Task<IActionResult> UserProfile()
        {
         var result = await  _userService.GetProfile();
            if (result == null)
                return BadRequest();

            return Ok(result);
        }

        [HttpPut("profile-update")]
        [SwaggerOperation(Summary = "Get profile details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Return UserProfile")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
        public async Task<IActionResult> UpdateProfile(UpdateUser user)
        {
            if (ModelState.IsValid)
            {
             var result = await _userService.UpdateInformation(user);
                if (result.Item1)
                {
                    return Ok(result.Item2);
                }
                return BadRequest(result.Item2);
            }
            return BadRequest("Unable to complete");
        }



    }
}
