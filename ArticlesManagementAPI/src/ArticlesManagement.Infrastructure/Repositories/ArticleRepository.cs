using ArticlesManagement.Domain.Abstractions;
using ArticlesManagement.Domain.Entities;
using ArticlesManagement.Domain.Enums;
using ArticlesManagement.Infrastructure.Extensions;
using ArticlesManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Infrastructure.Repositories
{
    public class ArticleRepository : BaseEntityRepository<Article>, IArticleRepository
    {
        protected readonly ApplicationDbContext _dbContext;

        public ArticleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Article>> GetMany(int? skip = null, int? take = null, int? authorId = null, ArticleType? type = null, string search = null)
        {
            var result = (await _dbContext.Articles
                    .OrderBy(a => a.PublishDate)
                    .ToListAsync())
                    .Where(a => a.PublishDate <= DateTime.UtcNow)
                    .SkipIf(skip)
                    .TakeIf(take != null, take.GetValueOrDefault())
                    .WhereIf(authorId != null, a => a.AuthorId == authorId)
                    .WhereIf(type != null, a => a.Type == type)
                    .WhereIf(!string.IsNullOrEmpty(search), a => a.Title.StartsWith(search))
                    .ToList();


            return result;
        }

        public async Task<List<ArticleOwner>> GetArticleOwners(int articleId)
        {
            return (await _dbContext.ArticleOwners
                .Include(ao => ao.Article)
                .Where(ao => ao.ArticleId == articleId)
                .ToListAsync());
        }

        public async Task<List<ArticleLike>> GetArticleLikes(int articleId)
        {
            return (await _dbContext.ArticleLikes
                .Where(ao => ao.ArticleId == articleId)
                .ToListAsync());
        }
    }
}

