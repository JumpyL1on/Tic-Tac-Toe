﻿using Common;
using Common.Requests;

namespace BusinessLogic.Interfaces
{
    public interface IPlayerService
    {
        public Task SignUpPlayerAsync(SignUpPlayerRequest request);
        public Task<TokensDTO> SignInPlayerAsync(SignInPlayerRequest request);
        public Task<TokensDTO> RefreshAccessTokenAsync(RefreshAccessTokenRequest request);
    }
}