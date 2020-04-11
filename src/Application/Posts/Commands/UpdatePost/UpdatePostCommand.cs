using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Application.Posts.Commands.UpdatePost
{
    public class UpdatePostCommand : IRequest<UpdatePostResponseDto>
    {
        [JsonIgnore]
        public int PostId { get; set; }

        public string Text { get; set; }

        public class UpdatePostCommandHandler
            : IRequestHandler<UpdatePostCommand, UpdatePostResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly IMapper _mapper;

            private readonly IStringLocalizer<PostsResource> _postLocalizer;

            private readonly IUserAccessor _userAccessor;

            public UpdatePostCommandHandler(IMapper mapper
                , IStringLocalizer<PostsResource> postLocalizer
                , IAppDbContext context
                , IUserAccessor userAccessor)
            {
                _mapper = mapper;
                _postLocalizer = postLocalizer;
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<UpdatePostResponseDto> Handle(UpdatePostCommand request
                , CancellationToken cancellationToken)
            {
                var post = await _context.Posts
                    .FirstOrDefaultAsync(p => p.Id == request.PostId
                                              && p.UserId == _userAccessor.UserId
                        , cancellationToken);
                if (post == null)
                {
                    throw new ValidationException(_postLocalizer["PostNull"]);
                }

                post.Text = request.Text;
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<UpdatePostResponseDto>(post);
            }
        }
    }
}