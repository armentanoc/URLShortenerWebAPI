using Microsoft.AspNetCore.Mvc;
using URLShortener.Application.Interfaces;
using URLShortener.ViewModels;

namespace URLShortener.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly ILogger<UrlController> _logger;
        private readonly IUrlService _urlService;

        public UrlController(ILogger<UrlController> logger, IUrlService urlService)
        {
            _logger = logger;
            _urlService = urlService;
        }

        public async Task<IActionResult> Get(string shortenedUrl)
        {
            var originalUrl = await _urlService.GetOriginalUrlAsync(shortenedUrl);

            if (originalUrl != null)
                return Ok(new { OriginalUrl = originalUrl });

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UrlDTO url)
        {
            if (url == null || string.IsNullOrEmpty(url.OriginalUrl))
                return BadRequest("Invalid input data");

            var shortenedUrl = await _urlService.ShortenUrlAsync(url.OriginalUrl);

            if (string.IsNullOrEmpty(shortenedUrl.ShortenedUrl))
                return BadRequest("Failed to create shortened URL");

            return CreatedAtAction(nameof(Get), new { shortenedUrl }, url);
        }
    }
}
