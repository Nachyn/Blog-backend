using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Blog.Localization
{
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<LocalizedIdentityErrorDescriber> _localizer;

        public LocalizedIdentityErrorDescriber(
            IStringLocalizer<LocalizedIdentityErrorDescriber> localizer)
        {
            _localizer = localizer;
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = "ConcurrencyFailure",
                Description = _localizer["ConcurrencyFailure"]
            };
        }

        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = "DefaultError", Description = _localizer["DefaultError"]
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = "DuplicateEmail",
                Description = _localizer["DuplicateEmail", email]
            };
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = "DuplicateRoleName",
                Description = _localizer["DuplicateRoleName", role]
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = "DuplicateUserName",
                Description = _localizer["DuplicateUserName", userName]
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = "InvalidEmail", Description = _localizer["InvalidEmail", email]
            };
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError
            {
                Code = "InvalidRoleName",
                Description =
                    _localizer["InvalidRoleName", role]
            };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = "InvalidToken", Description = _localizer["InvalidToken"]
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = "InvalidUserName",
                Description =
                    _localizer["InvalidUserName", userName]
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = "LoginAlreadyAssociated",
                Description = _localizer["LoginAlreadyAssociated"]
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = "PasswordMismatch",
                Description = _localizer["PasswordMismatch"]
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresDigit",
                Description = _localizer["PasswordRequiresDigit"]
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresLower",
                Description =
                    _localizer["PasswordRequiresLower"]
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description =
                    _localizer["PasswordRequiresNonAlphanumeric"]
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = "PasswordRequiresUniqueChars",
                Description =
                    _localizer["PasswordRequiresUniqueChars", uniqueChars]
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresUpper",
                Description =
                    _localizer["PasswordRequiresUpper"]
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = "PasswordTooShort",
                Description =
                    _localizer["PasswordTooShort", length]
            };
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError
            {
                Code = "RecoveryCodeRedemptionFailed",
                Description = _localizer["RecoveryCodeRedemptionFailed"]
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = "UserAlreadyHasPassword",
                Description = _localizer["UserAlreadyHasPassword"]
            };
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = "UserAlreadyInRole",
                Description = _localizer["UserAlreadyInRole", role]
            };
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
            {
                Code = "UserLockoutNotEnabled",
                Description = _localizer["UserLockoutNotEnabled"]
            };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
            {
                Code = "UserNotInRole",
                Description = _localizer["UserNotInRole", role]
            };
        }
    }
}