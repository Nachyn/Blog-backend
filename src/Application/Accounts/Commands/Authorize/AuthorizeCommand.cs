using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Accounts.Commands.Authorize
{
    public class AuthorizeCommand : IRequest<AuthorizeResponseDto>
    {
        public string Type { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string RefreshToken { get; set; }

        public class AuthorizeCommandHandler
            : IRequestHandler<AuthorizeCommand, AuthorizeResponseDto>
        {
            private readonly IMapper _mapper;

            private readonly IStringLocalizer<AccountsResource> _accountLocalizer;

            private readonly UserManager<AppUser> _userManager;

            private readonly IAppDbContext _context;

            private readonly IDateTime _dateTime;

            private readonly AuthOptions _authOptions;

            public AuthorizeCommandHandler(IMapper mapper
                , IOptions<AuthOptions> authOptions
                , IStringLocalizer<AccountsResource> accountLocalizer
                , UserManager<AppUser> userManager
                , IAppDbContext context
                , IDateTime dateTime)
            {
                _mapper = mapper;
                _accountLocalizer = accountLocalizer;
                _userManager = userManager;
                _context = context;
                _dateTime = dateTime;
                _authOptions = authOptions.Value;
            }

            public async Task<AuthorizeResponseDto> Handle(AuthorizeCommand request
                , CancellationToken cancellationToken)
            {
                return request.Type.ToLower() switch
                {
                    Token.TypePassword => await GenerateNewTokenUsingPasswordAsync(
                        request, cancellationToken),

                    Token.TypeRefresh => await GenerateNewTokenUsingRefreshAsync(request,
                        cancellationToken),

                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            private async Task<AuthorizeResponseDto> GenerateNewTokenUsingRefreshAsync(
                AuthorizeCommand request
                , CancellationToken cancellationToken)
            {
                AppUser user;
                try
                {
                    var range = ..request.RefreshToken.IndexOf('-');
                    var id = request.RefreshToken[range];
                    user = await _userManager.FindByIdAsync(id);

                    if (user == null)
                    {
                        throw new ValidationException(_accountLocalizer["UnableLogin"]);
                    }
                }
                catch
                {
                    throw new ValidationException(
                        _accountLocalizer["RefreshTokenInvalid"]);
                }

                var token = await _context.Tokens.FirstOrDefaultAsync(t =>
                    t.UserId == user.Id
                    && t.Client == _authOptions.Audience
                    && t.Value == request.RefreshToken);

                if (token == null || token.ExpiryTimeUtc <= _dateTime.NowUtc)
                {
                    throw new ValidationException(
                        _accountLocalizer["RefreshTokenInvalid"]);
                }

                var refreshToken = await CreateRefreshTokenAsync(user.Id
                    , cancellationToken
                    , token);

                return await CreateAccessTokenAsync(user, refreshToken);
            }

            private async Task<AuthorizeResponseDto> GenerateNewTokenUsingPasswordAsync(
                AuthorizeCommand request
                , CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null ||
                    !await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    throw new ValidationException(_accountLocalizer["UnableLogin"]);
                }

                var refreshToken =
                    await CreateRefreshTokenAsync(user.Id, cancellationToken);

                return await CreateAccessTokenAsync(user, refreshToken);
            }

            private async Task<AuthorizeResponseDto> CreateAccessTokenAsync(AppUser user
                , Token refreshToken)
            {
                if (user == null)
                {
                    throw new ArgumentException();
                }

                var roles = await _userManager.GetRolesAsync(user);
                var tokenHandler = new JwtSecurityTokenHandler();
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti
                        , Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim("LoggedOn"
                        , _dateTime.NowUtc.ToString(CultureInfo.InvariantCulture))
                };

                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    SigningCredentials =
                        new SigningCredentials(_authOptions.SymmetricSecurityKey,
                            SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _authOptions.Issuer,
                    Audience = _authOptions.Audience,
                    Expires =
                        _dateTime.NowUtc.AddMinutes(_authOptions.ExpiryTimeTokenMinutes)
                };

                var newToken = tokenHandler.CreateToken(tokenDescriptor);
                var encodedToken = tokenHandler.WriteToken(newToken);

                var authorizeResponse = _mapper.Map<AuthorizeResponseDto>(user);
                authorizeResponse.Token = encodedToken;
                authorizeResponse.ExpirationTokenUtc = newToken.ValidTo;
                authorizeResponse.RefreshToken = refreshToken.Value;
                authorizeResponse.ExpirationRefreshTokenUtc = refreshToken.ExpiryTimeUtc;
                authorizeResponse.Roles = roles;
                return authorizeResponse;
            }

            private async Task<Token> CreateRefreshTokenAsync(int userId
                , CancellationToken cancellationToken
                , Token oldToken = null)
            {
                if (userId < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(userId));
                }

                var token = oldToken ?? await _context.Tokens.FirstOrDefaultAsync(t =>
                        t.UserId == userId
                        && t.Client == _authOptions.Audience
                    , cancellationToken);

                var expiryTimeUtc = _dateTime.NowUtc.AddMinutes(_authOptions
                    .ExpiryTimeRefreshTokenMinutes);

                var tokenValue = $"{userId}-{Guid.NewGuid():D}";

                if (token == null)
                {
                    var newToken = new Token
                    {
                        Client = _authOptions.Audience,
                        ExpiryTimeUtc = expiryTimeUtc,
                        UserId = userId,
                        Value = tokenValue
                    };

                    _context.Tokens.Add(newToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return newToken;
                }

                token.ExpiryTimeUtc = expiryTimeUtc;
                token.Value = tokenValue;
                token.UpdatedUtc = _dateTime.NowUtc;
                await _context.SaveChangesAsync(cancellationToken);
                return token;
            }
        }
    }
}