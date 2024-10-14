using ArticlesManagement.Application.Abstractions;
using ArticlesManagement.Application.Helpers;
using ArticlesManagement.Application.Models.Requests;
using ArticlesManagement.Application.Models.Results;
using ArticlesManagement.Domain.Entities;
using ArticlesManagement.Domain.Enums;
using ArticlesManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArticlesManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IExecutionInfo _executionInfo;



        public ArticlesController(IArticleService articleService, IExecutionInfo executionInfo)
        {
            _articleService = articleService;
            _executionInfo = executionInfo;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles(
            [FromQuery] int? skip = 0,
            [FromQuery] int? take = 20,
            [FromQuery] ArticleType? type = null,
            [FromQuery] int? authorId = null,
            [FromQuery] string? search = null)
        {

            var articles = await _articleService.GetArticles(skip, take, authorId, type, search);
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(int id)
        {

            var article = await _articleService.GetArticleById(id);
            if (article == null)
                return NotFound();


            return Ok(article);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleRequest request)
        {

            if (_executionInfo.UserId == null)
            {
                return Unauthorized();
            }

            var userId = _executionInfo.UserId.Value;

            var result = await _articleService.CreateArticle(userId, request);

            if (result.Errors != null && result.Errors.Any())
            {
                return new BadRequestObjectResult(result);
            }
            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditArticle(int id, [FromBody] UpdateArticleRequest request)
        {

            if (_executionInfo.UserId == null)
            {
                return Unauthorized();
            }

            var userId = _executionInfo.UserId.Value;

            var updatedArticle = await _articleService.UpdateArticle(userId, id,  request);
            if (updatedArticle == null)
                return NotFound();

            return Ok(updatedArticle);
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            if (_executionInfo.UserId == null)
            {
                return Unauthorized();
            }

            var userId = _executionInfo.UserId.Value;

            await _articleService.DeleteArticle(userId,id);
            return NoContent();
        }

        [HttpPatch("read")]
        public async Task<IActionResult> ReadArticle(int id)
        { 
            await _articleService.ReadArticle(id);

            return Ok();
        }


        [Authorize]
        [HttpPost("like")]
        public async Task<IActionResult> LikeArticle(int articleId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value);

            await _articleService.LikeArticle(articleId, userId);

            return Ok();
        }

        //[Authorize]
        //[HttpPost("add-owners")]
        //public async Task<IActionResult> AddOwnersToArticle([FromBody] AddOwnersRequest request)
        //{
        //    // Get the current userId from the JWT token
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        //    if (userIdClaim == null)
        //    {
        //        return Unauthorized();
        //    }

        //    int userId = int.Parse(userIdClaim.Value);

        //    // Call the service method to add owners
        //    var result = await _articleService.AddOwnersToArticle(userId, request.ArticleId, request.NewOwnerUserIds);

        //    if (result.Errors != null && result.Errors.Any())
        //    {
        //        return BadRequest(new { Errors = result.Errors });
        //    }

        //    return Ok(new { Result = result.Result });
        //}

        //[HttpPost("comment")]
        //public async Task<IActionResult> CommentArticle(int articleId, CreateCommentRequest comment)
        //{
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        //    if (userIdClaim == null)
        //    {
        //        return Unauthorized();
        //    }

        //    int userId = int.Parse(userIdClaim.Value);

        //    var result = await _articleService.CommentArticle(articleId, userId, comment);

        //    if (result.Errors != null && result.Errors.Any())
        //    {
        //        return BadRequest(new ApiResponse { Errors = result.Errors });
        //    }

        //    return Ok(new BaseResult { Result = result.Result });
        //}
    }
}
