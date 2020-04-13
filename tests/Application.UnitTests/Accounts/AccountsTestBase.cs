using Application.Accounts;
using Application.Common.AppSettingHelpers.Entities;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Application.UnitTests.Accounts
{
    public class AccountsTestBase : CommonTestBase
    {
        protected IStringLocalizer<AccountsResource> AccountLocalizer;

        protected IOptions<AppUserSettings> AppUserSettingOptions;

        protected IOptions<AuthOptions> AuthOptionsOptions;

        protected IEmailService EmailService;

        public AccountsTestBase()
        {
            AuthOptionsOptions = Options.Create(Configuration
                .GetSection(nameof(AuthOptions))
                .Get<AuthOptions>());

            AppUserSettingOptions = Options.Create(Configuration
                .GetSection("EntitySettings:AppUser")
                .Get<AppUserSettings>());

            AccountLocalizer = TestHelpers.MockLocalizer<AccountsResource>();

            EmailService = Substitute.For<IEmailService>();
        }
    }
}