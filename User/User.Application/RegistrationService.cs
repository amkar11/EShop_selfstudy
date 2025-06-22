using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using User.Domain.Models;
using User.Domain.Repositories;
namespace User.Application
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserRepository _repository;
        public RegistrationService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> CheckIfUserExistsByUsernameAsync(string usernameOrEmail)
        {
            var user = await _repository.GetUserByNameOrEmailAsync(usernameOrEmail);
            if (user != null)
            {
                if (usernameOrEmail.Contains("@"))
                {
                    throw new InvalidOperationException($"User with such email {usernameOrEmail} already exists in {nameof(UserDb)}");
                }
                else throw new InvalidOperationException($"User with such username {usernameOrEmail} already exists in {nameof(UserDb)}");
            }
            return true;
        }

       

        public async Task AddNewUserToDatabaseAsync(UserDb user)
        {
            if (await CheckIfUserExistsByUsernameAsync(user.Name))
            {
                await _repository.AddUserAsync(user);
            }
        }
    }
}
