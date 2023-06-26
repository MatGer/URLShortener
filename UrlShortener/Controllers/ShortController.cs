using Microsoft.AspNetCore.Http.Extensions;
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

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Shortener(Links link)
        {
            var a = Request.GetDisplayUrl();
            var links = _linksDbContext.Links.Where(x=>x.LongUrl==link.LongUrl).FirstOrDefault();
            if (links!=null){
                TempData["NewLink"] = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{links.ShortUrl}";
                return RedirectToAction("Index", "Short");
            }
            var rand = new Random();
            Links obj= new Links();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            string randomString = new string(Enumerable.Repeat(chars, 5).Select(s => s[rand.Next(s.Length)]).ToArray());   //link length = 8 characters
            obj.LongUrl = link.LongUrl;
            obj.ShortUrl = randomString;
            _linksDbContext.Links.Add(obj);
            _linksDbContext.SaveChanges();
            TempData["NewLink"] = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{obj.ShortUrl}";
            return RedirectToAction("Index", "Short");
        }
    }
}