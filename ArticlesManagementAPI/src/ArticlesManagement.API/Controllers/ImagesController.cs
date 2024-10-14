using ArticlesManagement.Application.Abstractions;
using ArticlesManagement.Application.Interfaces;
using ArticlesManagement.Application.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArticlesManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService _imageService)
        {
            _imageService = _imageService;
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No image file uploaded.");
            }

            try
            {
                var imageUrl = await _imageService.UploadImageAsync(imageFile);
                return Ok(new { Url = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Image upload failed: {ex.Message}");
            }
        }
    }
}
