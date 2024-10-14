using ArticlesManagement.Application.Models.Requests;
using ArticlesManagement.Application.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> Register(RegisterRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        Task<bool> VerifyEmail(VerifyEmailRequest request);
        Task<string> RegenerateVerificationCode(string userEmail);

        Task<GetUserResponse> GetUserById(int id);
    }
}
