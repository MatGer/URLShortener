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
            //TempData["NewLink"] is used to show the generated link or error messages
            //check if field is null
            if (link.LongUrl is null)
            {
                TempData["NewLink"]="Please paste a link...";
                return RedirectToAction("Index", "Short");
            }
            //some validation
            if (link.LongUrl.Contains("drop") || link.LongUrl.Contains("select") || link.LongUrl.Contains("update") || !link.LongUrl.Contains("://"))
            {
                TempData["NewLink"] = "This is not a link!";
                return RedirectToAction("Index", "Short");
            }
            //if link already exists in database
            var longlink = _linksDbContext.Links.Where(x=>x.LongUrl==link.LongUrl).FirstOrDefault();
            if (longlink!=null){
                TempData["NewLink"] = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{longlink.ShortUrl}";
                return RedirectToAction("Index", "Short");
            }
            //procedure to create a new link
            var rand = new Random();
            Links obj = new Links();
            string randomString; //used to store the alias of the link (given or automatically generated)
            //if there is a custom alias
            if (link.ShortUrl != null)
            {
                link.ShortUrl=link.ShortUrl.Replace(" ","");
                if (link.ShortUrl.Length!=5)    //because our shortener uses a 5 character alias only!
                {
                    TempData["NewLink"] = "Alias must be exactly 5 characters long. Please type a valid one or leave it empty!";
                    return RedirectToAction("Index", "Short");
                }
                var shortlink = _linksDbContext.Links.Where(x=>x.ShortUrl==link.ShortUrl).FirstOrDefault();
                if (shortlink != null)  //if given alias already already exists in database
                {
                    TempData["NewLink"] = "This alias is used by another link. Please type a different one!";
                    return RedirectToAction("Index", "Short");
                }
                randomString = link.ShortUrl;
            }
            else
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                randomString = new string(Enumerable.Repeat(chars, 5).Select(s => s[rand.Next(s.Length)]).ToArray());   //link length = 5 characters
            }
            obj.LongUrl = link.LongUrl;
            obj.ShortUrl = randomString;
            _linksDbContext.Links.Add(obj);
            _linksDbContext.SaveChanges();
            TempData["NewLink"] = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{obj.ShortUrl}";
            return RedirectToAction("Index", "Short");
        }
    }
}