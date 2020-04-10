using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<CreatePostResponseDto>, IMapFrom<Post>
    {
        public string Text { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePostCommand, Post>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.UserId, opt => opt.Ignore())
                .ForMember(d => d.User, opt => opt.Ignore())
                .ForMember(d => d.LastModifiedUtc, opt => opt.Ignore())
                .ForMember(d => d.LoadedUtc, opt => opt.Ignore());
        }

        public class CreatePostCommandHandler
            : IRequestHandler<CreatePostCommand, CreatePostResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly IMapper _mapper;

            private readonly IUserAccessor _userAccessor;

            public CreatePostCommandHandler(IMapper mapper
                , IAppDbContext context
                , IUserAccessor userAccessor)
            {
                _mapper = mapper;
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<CreatePostResponseDto> Handle(CreatePostCommand request
                , CancellationToken cancellationToken)
            {
                var post = _mapper.Map<Post>(request);
                post.UserId = _userAccessor.UserId;

                _context.Posts.Add(post);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CreatePostResponseDto>(post);
            }
        }
    }
}