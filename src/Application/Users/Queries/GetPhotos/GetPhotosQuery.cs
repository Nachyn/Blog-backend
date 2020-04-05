using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Dtos;
using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Users.Queries.GetPhotos
{
    public class GetPhotosQuery : PaginationRequestDto, IRequest<GetPhotosResponseDto>
    {
        [FromRoute]
        public int UserId { get; set; }

        public GetPhotosPhotoSort Sort { get; set; }

        public class GetPhotosQueryHandler : IRequestHandler<GetPhotosQuery, GetPhotosResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly IMapper _mapper;

            private readonly IStringLocalizer<UsersResource> _userLocalizer;

            public GetPhotosQueryHandler(IAppDbContext context
                , IStringLocalizer<UsersResource> userLocalizer
                , IMapper mapper)
            {
                _context = context;
                _userLocalizer = userLocalizer;
                _mapper = mapper;
            }

            public async Task<GetPhotosResponseDto> Handle(GetPhotosQuery request
                , CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId
                    , cancellationToken);
                if (user == null)
                {
                    throw new ValidationException(_userLocalizer["UserNull"]);
                }

                var sortedQuery = GetUserPhotosSortedQueryable(_context.UserPhotos
                    .Where(p => p.UserId == user.Id), request.Sort);

                var userPhotos = await sortedQuery.Skip((request.NumberPage - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                return new GetPhotosResponseDto
                {
                    CountAllPages = (int) Math.Ceiling(
                        _context.UserPhotos.Count(p => p.UserId == user.Id) /
                        (double) request.PageSize),
                    CurrentPage = request.NumberPage,
                    Photos = _mapper.Map<List<GetPhotosUserPhotoDto>>(userPhotos)
                };
            }

            private IQueryable<AppUserPhoto> GetUserPhotosSortedQueryable(
                IQueryable<AppUserPhoto> query, GetPhotosPhotoSort sort)
            {
                return sort switch
                {
                    GetPhotosPhotoSort.LoadedAsc => query.AppendOrderBy(p => p.LoadedUtc),

                    GetPhotosPhotoSort.LoadedDesc =>
                    query.AppendOrderByDescending(p => p.LoadedUtc),

                    _ => throw new InvalidOperationException(nameof(GetPhotosPhotoSort))
                };
            }
        }
    }
}