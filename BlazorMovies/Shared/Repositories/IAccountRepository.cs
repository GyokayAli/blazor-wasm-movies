﻿using BlazorMovies.Shared.DTO;
using System.Threading.Tasks;

namespace BlazorMovies.Shared.Repositories
{
    public interface IAccountRepository
    {
        Task<UserToken> Login(UserInfo userInfo);
        Task<UserToken> Register(UserInfo userInfo);
    }
}
