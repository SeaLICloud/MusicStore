using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MusicStoreProject.Models;


//创建强类型控制器
namespace MusicStoreProject.Controllers
{
    [Authorize(Roles = "Administrator")]

    public class StoreManagerController : Controller
    {
        private readonly MusicStoreEntities _db = new MusicStoreEntities();



        public ViewResult Index()
        {
            var albums = _db.Albums
                .Include(a => a.Genre)
                .Include(a => a.Artist);
            //预先加载

            return View(albums.ToList());
            //变成列表,传给视图
        }


        public ViewResult Details(int id)
        {
            Album album = _db.Albums.Find(id);
            return View(album);
        }

        //get请求：用来给浏览器返回一个创建新专辑的页面
        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name");
            ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
            return View();
        } 

        //post请求：填完新专辑的信息后提交时返回给服务器
        [HttpPost]
        public ActionResult Create(Album album)//拿到Album对象
        {
            if (ModelState.IsValid)//判断是否该对象满足要求
            {
                _db.Albums.Add(album);
                _db.SaveChanges();//写到数据库
                return RedirectToAction("Index");  //回到Index页面显示会显示出新创建的Album
            }

            ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name", album.ArtistId);
            //实现创建专辑的的时候Genre Artist 的下拉列表
            //两个 SelectList 对象通过 ViewBag 传递给视图
            return View(album);
            //如果没有通过检查,重新显示创建页面
        }
        
 
        //get编辑：
        public ActionResult Edit(int id)
        {
            Album album = _db.Albums.Find(id);
            ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }


        //post编辑：
        [HttpPost]
        public ActionResult Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(album).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

 


        public ActionResult Delete(int id)
        {
            Album album = _db.Albums.Find(id);
            return View(album);
        }



        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Album album = _db.Albums.Find(id);
            _db.Albums.Remove(album);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //回收资源
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();//更改完成后关闭数据库
            base.Dispose(disposing);
        }
    }
}