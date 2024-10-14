using ArticlesManagement.Domain.Entities;
using ArticlesManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Domain.Abstractions
{
    public interface IArticleRepository: IBaseEntityRepository<Article>
    {
        Task<List<Article>> GetMany(int? skip = null, int? take = null, int? authorId = null, ArticleType? type = null, string search = null);

        Task<List<ArticleOwner>> GetArticleOwners(int articleId);

        Task<List<ArticleLike>> GetArticleLikes(int articleId);

    }
}
