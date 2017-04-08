using System.Linq;
using System.Web.Mvc;
using MusicStoreProject.Models;
using MusicStoreProject.ViewModels;

namespace MusicStoreProject.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly MusicStoreEntities _storeDb = new MusicStoreEntities();



        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }


        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedAlbum = _storeDb.Albums
                .Single(album => album.AlbumId == id);
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedAlbum);
            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(HttpContext);
            // Get the name of the album to display confirmation
            var albumName = _storeDb.Carts
                .Single(item => item.RecordId == id).Album.Title;
            // Remove from cart
            var itemCount = cart.RemoveFromCart(id);
            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(albumName) +
                          " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            //为Ajax使用的数据格式
            return Json(results);
        }


        [ChildActionOnly]//说明是一个分部视图,不能单独使用
        public ActionResult CartSummary()
        {
            //获取购物车
            var cart = ShoppingCart.GetCart(HttpContext);
            //传递到视图
            ViewData["CartCount"] = cart.GetCount();
            //使用部分视图
            return PartialView("CartSummary");

        }
    }
}