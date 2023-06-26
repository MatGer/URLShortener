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
            if (link.LongUrl is null)
            {
                TempData["NewLink"]="Please paste a link...";
                return RedirectToAction("Index", "Short");
            }
            if (link.LongUrl.Contains("drop") || link.LongUrl.Contains("select") || link.LongUrl.Contains("update") || !link.LongUrl.Contains("://"))
            {
                TempData["NewLink"] = "This is not a link!";
                return RedirectToAction("Index", "Short");
            }
            var links = _linksDbContext.Links.Where(x=>x.LongUrl==link.LongUrl).FirstOrDefault();
            if (links!=null){
                TempData["NewLink"] = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{links.ShortUrl}";
                return RedirectToAction("Index", "Short");
            }
            var rand = new Random();
            Links obj= new Links();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            string randomString = new string(Enumerable.Repeat(chars, 5).Select(s => s[rand.Next(s.Length)]).ToArray());   //link length = 5 characters
            obj.LongUrl = link.LongUrl;
            obj.ShortUrl = randomString;
            _linksDbContext.Links.Add(obj);
            _linksDbContext.SaveChanges();
            TempData["NewLink"] = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{obj.ShortUrl}";
            return RedirectToAction("Index", "Short");
        }
    }
}