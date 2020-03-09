﻿using System.Collections.Generic;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountUserInfo : IMapFrom<AppUser>
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, CreateAccountUserInfo>()
                .ForMember(d => d.Roles, opt => opt.Ignore());
        }
    }
}