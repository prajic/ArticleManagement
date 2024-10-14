using ArticlesManagement.Application.Models.Requests;
using ArticlesManagement.Application.Models.Responses;
using ArticlesManagement.Application.Models.Results;
using ArticlesManagement.Domain.Entities;
using ArticlesManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Application.Abstractions
{
    public interface IArticleService
    {
        Task<List<Article>> GetArticles(int? skip = 0, int? take = 10, int? authorId = null, ArticleType? type = null, string? search = null);
        Task<Article> GetArticleById(int id);
        Task<BaseResult<GetArticleResponse>> CreateArticle(int userId, CreateArticleRequest request);
        Task<BaseResult<GetArticleResponse>> UpdateArticle(int userId, int articleId, UpdateArticleRequest request);
        Task<BaseResult<Article>> AddOwnersToArticle(int userId, int articleId, List<int> newOwnerUserIds);
        Task DeleteArticle(int userId, int id);
        Task ReadArticle(int id);
        Task LikeArticle(int id, int userId);
    }
}
