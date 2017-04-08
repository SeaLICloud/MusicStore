using System.Data.Entity;

namespace MusicStoreProject.Models
{
    public class MusicStoreEntities: DbContext
        //派生类,基类为数据库上下文
        //需要引用System.Data.Entity
        //此类为程序与数据库沟通的桥梁
    {

        //以下每一个属性对应数据库的一个表
        public DbSet<Album> Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
    
}