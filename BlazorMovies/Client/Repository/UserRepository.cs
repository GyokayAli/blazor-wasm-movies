using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/users";

        public UserRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<PaginatedResponse<List<UserDTO>>> GetUsers(PaginationDTO pagination)
        {
            return await _httpService.GetHelper<List<UserDTO>>(url, pagination);
        }

        public async Task<List<RoleDTO>> GetRoles()
        {
            return await _httpService.GetHelper<List<RoleDTO>>($"{url}/roles");
        }

        public async Task AssignRole(EditRoleDTO editRole)
        {
            var response = await _httpService.Post($"{url}/assignRole", editRole);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task RemoveRole(EditRoleDTO editRole)
        {
            var response = await _httpService.Post($"{url}/removeRole", editRole);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
