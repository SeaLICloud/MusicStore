using System.Linq;
using System.Web.Mvc;
using MusicStoreProject.Models;

namespace MusicStoreProject.Controllers
{
    public class StoreController : Controller
    {
        //用于访问数据库的私有成员
        //就通过这个数据库上下文的对象
        //这里标记为只读
        private readonly MusicStoreEntities _storeDb=new MusicStoreEntities();
        
        
        //索引
        public ActionResult Index ()
        {
            //现在使用数据库,ToList为访问一个列表。
            var genres = _storeDb.Genres.ToList();
            return View(genres);
        }



        //浏览
        public ActionResult Browse(string genre)
        {
            var genreModel = _storeDb.Genres
                .Include("Albums")
                .Single(g => g.Name == genre);

            return View(genreModel); 
            //属于这个流派的专辑集合
            //通过 Include 方法可以指定我们希望获取的相关信息
            //我们就可以在一次数据访问中,既可以获取流派对象,也可以同时获取相关的专辑对象。
        }



        //详细
        public ActionResult Details(int id)
        {
            var album = _storeDb.Albums.Find(id);
            return View(album);
        }



        //分类菜单
        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = _storeDb.Genres.ToList();
            return PartialView(genres);
        }
    }
}