
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class Links
    {
        [Key]
        public int Id { get; set; }
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }

    }
}
