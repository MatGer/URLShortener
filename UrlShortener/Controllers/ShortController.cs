using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UrlShortener.Models;


namespace UrlShortener.Controllers
{
    public class ShortController : Controller
    {
        private readonly ILogger<ShortController> _logger;
        private readonly LinksDbContext _linksDbContext;

        public ShortController(ILogger<ShortController> logger, LinksDbContext linksDbContext)
        {
            _logger = logger;
            _linksDbContext = linksDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var links = await _linksDbContext.Links.ToListAsync();
            var json = new
            {
                data = links.Select(i => new
                {
                    id=i.Id,
                    lUrl=i.LongUrl,
                    sUrl = i.ShortUrl
                })
            };
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return NotFound();
        //}
    }
}