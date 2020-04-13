using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        #region "Fields"

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        #endregion

        #region "Constructor"

        public UsersController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        #endregion

        #region "GET Methods"

        /// <summary>
        /// Gets a paginated result of Users.
        /// </summary>
        /// <param name="pagination">The pagination options.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _dbContext.Users.AsQueryable();
            await HttpContext.InsertPaginationParamatersInResponse(queryable, pagination.RecordsPerPage);

            return await queryable.Paginate(pagination)
                .Select(x => new UserDTO { Email = x.Email, UserId = x.Id }).ToListAsync();
        }

        /// <summary>
        /// Gets all user roles.
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        public async Task<ActionResult<List<RoleDTO>>> Get()
        {
            return await _dbContext.Roles
                .Select(x => new RoleDTO { RoleName = x.Name }).ToListAsync();
        }
        #endregion

        #region "POST Methods"

        /// <summary>
        /// Assigns a role to User.
        /// </summary>
        /// <param name="editRoleDto">The edit role settings.</param>
        /// <returns></returns>
        [HttpPost("assignRole")]
        public async Task<ActionResult> AssignRole(EditRoleDTO editRoleDto)
        {
            var user = await _userManager.FindByIdAsync(editRoleDto.UserId);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDto.RoleName));
            return NoContent();
        }

        /// <summary>
        /// Removes a role for User.
        /// </summary>
        /// <param name="editRoleDto">The edit role settings.</param>
        /// <returns></returns>
        [HttpPost("removeRole")]
        public async Task<ActionResult> RemoveRole(EditRoleDTO editRoleDto)
        {
            var user = await _userManager.FindByIdAsync(editRoleDto.UserId);
            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDto.RoleName));
            return NoContent();
        }
        #endregion
    }
}