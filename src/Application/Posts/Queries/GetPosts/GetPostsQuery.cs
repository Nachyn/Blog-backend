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

namespace Application.Posts.Queries.GetPosts
{
    public class GetPostsQuery : PaginationRequestDto, IRequest<GetPostsResponseDto>
    {
        [FromRoute]
        public int UserId { get; set; }

        public GetPostsPostSort Sort { get; set; }

        public class GetPostsQueryHandler
            : IRequestHandler<GetPostsQuery, GetPostsResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly IMapper _mapper;

            private readonly IStringLocalizer<PostsResource> _postLocalizer;

            public GetPostsQueryHandler(IMapper mapper
                , IAppDbContext context
                , IStringLocalizer<PostsResource> postLocalizer)
            {
                _mapper = mapper;
                _context = context;
                _postLocalizer = postLocalizer;
            }

            public async Task<GetPostsResponseDto> Handle(GetPostsQuery request
                , CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == request.UserId
                        , cancellationToken);
                if (user == null)
                {
                    throw new ValidationException(_postLocalizer["UserNull"]);
                }

                var posts = await GetPostsSortedQueryable(_context.Posts
                        .Where(p => p.UserId == request.UserId), request.Sort)
                    .Skip((request.NumberPage - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var postFiles = await _context.PostFiles
                    .Where(f => posts.Select(p => p.Id).Contains(f.PostId))
                    .ToListAsync(cancellationToken);

                var postDtos = posts.Select(p =>
                {
                    var postDto = _mapper.Map<GetPostsPostDto>(p);

                    postDto.Files = _mapper.Map<List<GetPostsPostFileDto>>(
                        postFiles.Where(f => f.PostId == p.Id)
                            .OrderBy(f => f.LoadedUtc));

                    return postDto;
                }).ToList();

                return new GetPostsResponseDto
                {
                    Posts = postDtos,
                    CurrentPage = request.NumberPage,
                    CountAllPages = (int) Math.Ceiling(
                        _context.Posts.Count(p => p.UserId == user.Id) /
                        (double) request.PageSize)
                };
            }

            private IQueryable<Post> GetPostsSortedQueryable(
                IQueryable<Post> query, GetPostsPostSort sort)
            {
                return sort switch
                {
                    GetPostsPostSort.LoadedAsc => query.AppendOrderBy(p => p.LoadedUtc),

                    GetPostsPostSort.LoadedDesc =>
                    query.AppendOrderByDescending(p => p.LoadedUtc),

                    _ => throw new InvalidOperationException(nameof(GetPostsPostSort))
                };
            }
        }
    }
}