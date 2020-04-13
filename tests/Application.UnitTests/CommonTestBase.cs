using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Validators;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Application.UnitTests
{
    public class CommonTestBase
    {
        protected IStringLocalizer<CommonValidatorsResource> CommonLocalizer;

        protected IConfiguration Configuration;

        protected AppDbContext Context;

        protected IDateTime DateTimeService;

        protected string DefaultUserEmail;

        protected int DefaultUserId;

        protected string DefaultUserPassword;

        protected IMapper Mapper;

        protected RoleManager<IdentityRole<int>> RoleManager;

        protected UserManager<AppUser> UserManager;

        public CommonTestBase()
        {
            var configurationProvider = new MapperConfiguration(cfg =>
                cfg.AddProfile<MappingProfile>());

            configurationProvider.AssertConfigurationIsValid();
            Mapper = configurationProvider.CreateMapper();

            DateTimeService = new DateTimeService();
            Configuration = CreateConfiguration();
            InitializeDefaultFields();
            CommonLocalizer = TestHelpers.MockLocalizer<CommonValidatorsResource>();
        }

        protected EmptyConstraint IsNotNullOrEmpty => Is.Not.Null.And.Not.Empty;

        [SetUp]
        public virtual async Task InitializeDatabase()
        {
            Context = AppDbContextFactory.Create(DateTimeService);

            RoleManager = CreateRoleManager();
            UserManager = CreateUserManager();

            await AppDbContextSeed.InitializeAsync(UserManager
                , RoleManager
                , Configuration
                , Context);
        }

        [TearDown]
        public virtual void ClearDatabase()
        {
            AppDbContextFactory.Destroy(Context);
            UserManager.Dispose();
            RoleManager.Dispose();
        }

        private UserManager<AppUser> CreateUserManager()
        {
            var passwordIdentitySettings = Configuration
                .GetSection(nameof(PasswordIdentitySettings))
                .Get<PasswordIdentitySettings>();

            var userManager = new UserManager<AppUser>(
                new UserStore<AppUser, IdentityRole<int>, AppDbContext, int>(Context)
                , Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = passwordIdentitySettings.RequireDigit,
                        RequiredLength = passwordIdentitySettings.RequiredLength,
                        RequireLowercase = passwordIdentitySettings.RequireLowercase,
                        RequireUppercase = passwordIdentitySettings.RequireUppercase,
                        RequireNonAlphanumeric = passwordIdentitySettings.RequireUppercase
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true
                    },
                    Tokens = new TokenOptions
                    {
                        PasswordResetTokenProvider = TokenOptions.DefaultPhoneProvider
                    }
                })
                , new PasswordHasher<AppUser>()
                , new List<UserValidator<AppUser>> {new UserValidator<AppUser>()}
                , new List<PasswordValidator<AppUser>> {new PasswordValidator<AppUser>()}
                , new UpperInvariantLookupNormalizer()
                , new IdentityErrorDescriber()
                , null
                , Substitute.For<ILogger<UserManager<AppUser>>>());

            userManager.RegisterTokenProvider(TokenOptions.DefaultPhoneProvider
                , new PhoneNumberTokenProvider<AppUser>());

            return userManager;
        }

        private RoleManager<IdentityRole<int>> CreateRoleManager()
        {
            return new RoleManager<IdentityRole<int>>(
                new RoleStore<IdentityRole<int>, AppDbContext, int>(Context)
                , new List<RoleValidator<IdentityRole<int>>>
                    {new RoleValidator<IdentityRole<int>>()}
                , new UpperInvariantLookupNormalizer()
                , new IdentityErrorDescriber()
                , Substitute.For<ILogger<RoleManager<IdentityRole<int>>>>());
        }

        private IConfiguration CreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json");

            return builder.Build();
        }

        private void InitializeDefaultFields()
        {
            var adminAccount = Configuration
                .GetSection(nameof(AdminAccount))
                .Get<AdminAccount>();

            DefaultUserEmail = adminAccount.Email;
            DefaultUserPassword = adminAccount.Password;
            DefaultUserId = 1;
        }
    }
}