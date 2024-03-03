using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using URLShortener.Application.Interfaces;
using URLShortener.Domain;
using URLShortener.ViewModels;

namespace URLShortener.WebAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class UrlController : ControllerBase
    {
        private readonly ILogger<UrlController> _logger;
        private readonly IUrlService _urlService;
        private readonly IConfiguration _configuration;

        public UrlController(ILogger<UrlController> logger, IUrlService urlService, IConfiguration configuration)
        {
            _logger = logger;
            _urlService = urlService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("all")]
        [SwaggerOperation("Get all URLs in database.")]
        public async Task<IActionResult> GetAll()
        {
            var urls = await _urlService.GetAllAsync();

            if (urls is not null && urls.Any())
                return Ok(urls);

            return NotFound();
        }

        [HttpGet]
        [Route("{slug}")]
        [SwaggerOperation("Redirects to original URL by existing shortened URL.")]
        public async Task<IActionResult> Get([FromRoute] string slug)
        {
            try
            {
                Url originalUrl = await _urlService.GetOriginalUrlAsync(slug);

                if (originalUrl != null)
                {
                    _logger.LogInformation($"Redirecting to: {originalUrl.OriginalUrl}");
                    return Redirect(originalUrl.OriginalUrl);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving original URL: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        [Route("makeUrlShort")]
        [SwaggerOperation("Shortens a new URL and returns the object saved on database.")]
        public async Task<IActionResult> Add([FromBody] UrlRequest url)
        {
            if (url == null || string.IsNullOrEmpty(url.OriginalUrl))
                return BadRequest("Invalid input data");

            var shortenedUrl = await _urlService.ShortenUrlAsync(url.OriginalUrl);

            if (string.IsNullOrEmpty(shortenedUrl.ShortenedUrl))
                return BadRequest("Failed to create shortened URL");

            return CreatedAtAction(nameof(Get), new { slug = shortenedUrl.ShortenedUrl }, shortenedUrl);
        }
    }
}
