using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _dbContext.Users.AsQueryable();
            await HttpContext.InsertPaginationParamatersInResponse(queryable, pagination.RecordsPerPage);

            return await queryable.Paginate(pagination)
                .Select(x => new UserDTO { Email = x.Email, UserId = x.Id }).ToListAsync();
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<RoleDTO>>> Get()
        {
            return await _dbContext.Roles
                .Select(x => new RoleDTO { RoleName = x.Name }).ToListAsync();
        }

        [HttpPost("assignRole")]
        public async Task<ActionResult> AssignRole(EditRoleDTO editRoleDto)
        {
            var user = await _userManager.FindByIdAsync(editRoleDto.UserId);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDto.RoleName));
            return NoContent();
        }

        [HttpPost("removeRole")]
        public async Task<ActionResult> RemoveRole(EditRoleDTO editRoleDto)
        {
            var user = await _userManager.FindByIdAsync(editRoleDto.UserId);
            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDto.RoleName));
            return NoContent();
        }
    }
}