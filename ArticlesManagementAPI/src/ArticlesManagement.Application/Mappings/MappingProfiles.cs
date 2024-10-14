using ArticlesManagement.Application.Models.Requests;
using ArticlesManagement.Application.Models.Responses;
using ArticlesManagement.Domain.Entities;
using AutoMapper;


namespace ArticlesManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, GetUserResponse>();
            CreateMap<Author, GetAuthorResponse>();

            CreateMap<CreateArticleRequest, Article>()
                .ForMember(dest => dest.AuthorId, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ArticleOwners, opt => opt.MapFrom(src => src.OwnerIds.Select(userId => new ArticleOwner
                {
                    UserId = userId,
                })));

            CreateMap<UpdateArticleRequest, Article>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ArticleOwners, opt => opt.MapFrom(src => src.OwnerIds.Select(userId => new ArticleOwner
                 {
                     UserId = userId,
                     ArticleId = src.Id
                 })));

            CreateMap<Article, GetArticleResponse>()
                .ForMember(dest => dest.ArticleOwners, opt => opt.MapFrom(src => src.ArticleOwners.Select(ao => ao.UserId)));
        }
    }
}
