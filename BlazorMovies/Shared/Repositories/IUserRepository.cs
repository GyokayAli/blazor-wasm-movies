﻿using BlazorMovies.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Shared.Repositories
{
    public interface IUserRepository
    {
        Task<List<RoleDTO>> GetRoles();
        Task<PaginatedResponse<List<UserDTO>>> GetUsers(PaginationDTO pagination);
        Task AssignRole(EditRoleDTO editRole);
        Task RemoveRole(EditRoleDTO editRole);
    }
}
