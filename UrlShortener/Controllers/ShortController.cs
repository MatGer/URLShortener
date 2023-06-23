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
            //var links = await _linksDbContext.Links.ToListAsync();
            //var json = new
            //{
            //    data = links.Select(i => new
            //    {
            //        id=i.Id,
            //        lUrl=i.LongUrl,
            //        sUrl = i.ShortUrl
            //    })
            //};
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Shortener(Links link)
        {
            var links = _linksDbContext.Links.Where(x=>x.LongUrl==link.LongUrl).FirstOrDefault();
            if (links!=null){
                ViewBag.link = links;
                return RedirectToAction("Index", "Home");
            }
            var rand = new Random();
            Links obj= new Links();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            string randomString = new string(Enumerable.Repeat(chars, 8).Select(s => s[rand.Next(s.Length)]).ToArray());   //link length = 8 characters
            obj.LongUrl = link.LongUrl;
            obj.ShortUrl = randomString;
            _linksDbContext.Links.Add(obj);
            _linksDbContext.SaveChanges();
            ViewBag.link = obj;
            return RedirectToAction("Index", "Home");
        }
    }
}