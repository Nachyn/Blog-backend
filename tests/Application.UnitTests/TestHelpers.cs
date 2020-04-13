using Microsoft.Extensions.Localization;
using NSubstitute;

namespace Application.UnitTests
{
    public static class TestHelpers
    {
        public static IStringLocalizer<T> MockLocalizer<T>()
        {
            var localizer = Substitute.For<IStringLocalizer<T>>();

            localizer[default].ReturnsForAnyArgs(info =>
                new LocalizedString(info.Arg<string>(), info.Arg<string>()));

            localizer[default, default].ReturnsForAnyArgs(info =>
                new LocalizedString(info.Arg<string>(), info.Arg<string>()));

            return localizer;
        }
    }
}