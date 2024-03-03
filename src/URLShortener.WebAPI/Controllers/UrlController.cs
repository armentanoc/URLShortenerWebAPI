using Microsoft.AspNetCore.Mvc;
using URLShortener.Application.Interfaces;
using URLShortener.Domain;
using URLShortener.ViewModels;
using URLShortener.WebAPI.Middlewares;

namespace URLShortener.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        [Route("{shortenedUrl}")]
        public async Task<IActionResult> Get(string shortenedUrl)
        {
            Url originalUrl = await _urlService.GetOriginalUrlAsync(shortenedUrl);

            if (originalUrl is Url url)
                return Ok(url);

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UrlRequest url)
        {
            if (url == null || string.IsNullOrEmpty(url.OriginalUrl))
                return BadRequest("Invalid input data");

            var shortenedUrl = await _urlService.ShortenUrlAsync(url.OriginalUrl);

            if (string.IsNullOrEmpty(shortenedUrl.ShortenedUrl))
                return BadRequest("Failed to create shortened URL");

            return CreatedAtAction(nameof(Get), new { shortenedUrl }, shortenedUrl);
        }
    }
}
