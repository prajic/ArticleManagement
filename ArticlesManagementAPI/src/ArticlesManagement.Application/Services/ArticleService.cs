using ArticlesManagement.Application.Abstractions;
using ArticlesManagement.Application.Models.Requests;
using ArticlesManagement.Application.Models.Responses;
using ArticlesManagement.Application.Models.Results;
using ArticlesManagement.Domain.Abstractions;
using ArticlesManagement.Domain.Entities;
using ArticlesManagement.Domain.Enums;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        public ArticleService(IArticleRepository articleRepository, IMapper mapper, IUserRepository userRepository)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<Article>> GetArticles(int? skip = 0, int? take = 10, int? authorId = null, ArticleType? type = null, string? search = null)
        {
            var articles = await _articleRepository.GetMany(skip, take, authorId, type, search);

            return articles;
        }

        public async Task<Article> GetArticleById(int id)
        {
            return await _articleRepository.GetById(id);
        }

        public async Task<BaseResult<GetArticleResponse>> CreateArticle(int userId, CreateArticleRequest request)
        {
            var article = _mapper.Map<Article>(request);

            var user = await _userRepository.GetById(userId);

            if (user.Author == null)
            {
                return new BaseResult<GetArticleResponse> { 
                    Errors = new List<string>() { "You have to be an author to create an article"}
                };
            }

            var articleOwner = new ArticleOwner
            {

                UserId = userId,
                ArticleId = article.Id,
                Article = article,
                User = user
            };

            article.AuthorId = userId;
            article.ArticleOwners.Add(articleOwner);

            article = await _articleRepository.Create(article);

            var response = _mapper.Map<GetArticleResponse>(article);

            var result = new BaseResult<GetArticleResponse> { Result = response  };


            return result;
        }

        public async Task<BaseResult<GetArticleResponse>> UpdateArticle(int userId, int articleId, UpdateArticleRequest request)
        {
            var articleOwners = await _articleRepository.GetArticleOwners(articleId);
            var article = await _articleRepository.GetById(request.Id);

            if (article == null)
            {
                return new BaseResult<GetArticleResponse>
                {
                    Errors = new List<string> { "Article not found" }
                };
            }

            var user = await _userRepository.GetById(userId);

            if (!articleOwners.Select(ao => ao.UserId).Contains(userId))
            {

                foreach (var ao in articleOwners)
                {
                    // possibly implement feature that notifies owners of article that the updates are being made
                }

                return new BaseResult<GetArticleResponse>
                {
                    Errors = new List<string>() { "Only owners of article can edit article" }
                };
            }
            else
            {
                _mapper.Map(request, article);

                article = await _articleRepository.Update(article);
            }

            var response = _mapper.Map<GetArticleResponse>(article);

            var result = new BaseResult<GetArticleResponse> { Result = response };

            return result;
        }

        public async Task DeleteArticle(int userId, int articleId)
        {
            var article = await _articleRepository.GetById(articleId);

            var articleOwners = await _articleRepository.GetArticleOwners(articleId);

            if (!articleOwners.Select(ao => ao.UserId).Contains(userId))
            {
                return;
                // only owners can delete
            }


            if (article != null)
            {
                await _articleRepository.Remove(articleId);
            }
            else
            {
                throw new KeyNotFoundException("Article not found");
            }
        }

        // future features

        public async Task<BaseResult<Article>> AddOwnersToArticle(int userId, int articleId, List<int> newOwnerUserIds)
        {
            // Fetch the article
            var article = await _articleRepository.GetById(articleId);

            // Check if article exists
            if (article == null)
            {
                return new BaseResult<Article>
                {
                    Errors = new List<string> { "Article not found" }
                };
            }

            // Check if the user making the request is one of the current owners
            var currentUser = await _userRepository.GetById(userId);
            if (currentUser == null)
            {
                return new BaseResult<Article>
                {
                    Errors = new List<string> { "User not found" }
                };
            }

            var currentOwner = article.ArticleOwners.FirstOrDefault(ao => ao.UserId == userId);
            if (currentOwner == null)
            {
                return new BaseResult<Article>
                {
                    Errors = new List<string> { "Only current article owners can add new owners" }
                };
            }

            // Add the new owners
            foreach (var newOwnerId in newOwnerUserIds)
            {
                // Check if the new owner exists
                var newOwner = await _userRepository.GetById(newOwnerId);
                if (newOwner == null)
                {
                    return new BaseResult<Article>
                    {
                        Errors = new List<string> { $"User with ID {newOwnerId} not found" }
                    };
                }

                // Check if the user is already an owner
                if (!article.ArticleOwners.Any(ao => ao.UserId == newOwnerId))
                {
                    // Add the new owner
                    article.ArticleOwners.Add(new ArticleOwner
                    {
                        ArticleId = article.Id,
                        UserId = newOwnerId,
                        User = newOwner,
                        Article = article
                    });
                }
            }

            // Save changes to the article
            await _articleRepository.Update(article);

            return new BaseResult<Article>
            {
                Result = article
            };
        }

        public async Task<Article> ApproveOfArticleChanges(int userId, int editorId, UpdateArticleRequest request)
        {
            var article = _mapper.Map<Article>(request);

            var user = await _userRepository.GetById(userId);

            var articleOwner = new ArticleOwner
            {

                UserId = userId,
                ArticleId = article.Id,
                Article = article,
                User = user
            };

            if (!article.ArticleOwners.Contains(articleOwner))
            {
                article.ArticleOwners.Add(articleOwner);
                article = await _articleRepository.Update(article);

                // notify user that his/her edit has been approved
            }


            return article;
        }


        public async Task ReadArticle(int id)
        {
            var article = await _articleRepository.GetById(id);
            
            if (article != null)
            {
                article.ReadCount++;
                await _articleRepository.Update(article);
            }
        }

        public async Task LikeArticle(int articleId, int userId)
        {
            var article = await _articleRepository.GetById(articleId);
            var articleLikes = await _articleRepository.GetArticleLikes(articleId);

            if (article == null)
            {
                return;
            }

            article.ArticleLikes = articleLikes;

            var user = await _userRepository.GetById(userId);

            if (user == null)
            {
                return;
            }

            // Check if the user has already liked the article
            var existingLike = article.ArticleLikes.FirstOrDefault(l => l.UserId == userId);
            if (existingLike != null)
            {
                // If the user has already liked the article, remove the like
                article.ArticleLikes.Remove(existingLike);
            }
            else
            {
                // If the user hasn't liked the article, add a like
                article.ArticleLikes.Add(new ArticleLike { ArticleId = articleId, UserId = userId });
            }

            await _articleRepository.Update(article);
        }

        //public async Task<BaseResult<Article>> CommentArticle(int articleId, int userId, CreateCommentRequest comment)
        //{
        //    var article = await _articleRepository.GetById(articleId);

        //    if (article == null)
        //    {
        //        return new BaseResult<Article>
        //        {
        //            Errors = new List<string> { "Article not found" }
        //        };
        //    }

        //    var user = await _userRepository.GetById(userId);

        //    if (user == null)
        //    {
        //        return new BaseResult<Article>
        //        {
        //            Errors = new List<string> { "User not found" }
        //        };
        //    }

        //    var newComment = new ArticleComment
        //    {
        //        ArticleId = articleId,
        //        UserId = userId,
        //        Content = comment.Content,
        //        ParentCommentId = comment.ParentCommentId
        //    };

        //    article.ArticleComments.Add(newComment);

        //    await _articleRepository.Update(article);

        //    return new BaseResult<Article>
        //    {
        //        Result = article
        //    };
        //}
    }
}
