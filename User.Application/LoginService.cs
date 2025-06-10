using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;
using User.Domain.Models.Exceptions;
using User.Domain.PasswordHasher;
using User.Domain.Repositories;

namespace User.Application
{
    public class LoginService : ILoginService
    {
        private readonly IJwtTokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;
        public LoginService(IJwtTokenService tokenService, IUserRepository userRepository, IHasher hasher)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _hasher = hasher;
        }
        public async Task<string> LoginAsync(string username, string password, bool rememberMe = false)
        {
            UserDb user = await _userRepository.GetUserByNameAndPasswordAsync(username, password);
            List<string> roles = new List<string>();
            if (username == "aleksafleksa" && _hasher.Verify(user.PasswordHash, password))
            {
                List<string> admin_roles = new List<string> { "Client", "Employeer", "Administrator" };
                roles = new List<string>(admin_roles);
                
            }
            else if (username != "aleksafleksa" && _hasher.Verify(user.PasswordHash, password))
            {
                List<string> user_roles = new List<string> { "Client" };
                roles = new List<string>(user_roles);
            }
            else throw new InvalidCredentialsException();
            var access_token = _tokenService.GenerateToken(user.Id, roles, rememberMe);
            return access_token;
        }
    }
}
