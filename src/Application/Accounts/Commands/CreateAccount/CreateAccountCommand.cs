using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<CreateAccountUserInfoDto>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public class CreateAccountCommandHandler
            : IRequestHandler<CreateAccountCommand, CreateAccountUserInfoDto>
        {
            private readonly UserManager<AppUser> _userManager;

            private readonly IMapper _mapper;

            public CreateAccountCommandHandler(UserManager<AppUser> userManager
                , IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<CreateAccountUserInfoDto> Handle(CreateAccountCommand request
                , CancellationToken cancellationToken)
            {
                var user = new AppUser
                {
                    Email = request.Email, UserName = request.Username
                };

                var createResult = await _userManager.CreateAsync(user, request.Password);
                if (!createResult.Succeeded)
                {
                    throw new ValidationException(createResult);
                }

                var roleResult = await _userManager.AddToRoleAsync(user
                    , Roles.User.GetEnumDescription());

                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(
                        await _userManager.FindByEmailAsync(user.Email));

                    throw new Exception("error adding role to user");
                }

                var response = _mapper.Map<CreateAccountUserInfoDto>(user);
                response.Roles = (await _userManager.GetRolesAsync(user)).ToList();
                return response;
            }
        }
    }
}