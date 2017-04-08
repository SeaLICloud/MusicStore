using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MusicStoreProject.Models;

namespace MusicStoreProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicStoreEntities _storeDb = new MusicStoreEntities();

        public ActionResult Index()
        {
            List<Album> albums = GetTopSellingAlbums(5);
            return View(albums);
        }


        //非Action
        [NonAction]
        private List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return _storeDb.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }
    }
}
