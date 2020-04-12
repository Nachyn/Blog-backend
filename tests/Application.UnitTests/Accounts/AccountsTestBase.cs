using Application.Accounts;
using Application.Common.AppSettingHelpers.Main;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Application.UnitTests.Accounts
{
    public class AccountsTestBase : CommonTestBase
    {
        protected IStringLocalizer<AccountsResource> AccountLocalizer;

        protected IOptions<AuthOptions> AuthOptionsOptions;

        public AccountsTestBase()
        {
            AuthOptionsOptions = Options.Create(Configuration
                .GetSection(nameof(AuthOptions))
                .Get<AuthOptions>());

            AccountLocalizer = Substitute.For<IStringLocalizer<AccountsResource>>();
        }
    }
}