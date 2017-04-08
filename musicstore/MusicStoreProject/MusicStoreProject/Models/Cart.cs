using System.ComponentModel.DataAnnotations;

namespace MusicStoreProject.Models
{
    public class Cart
    {
            [Key]
            public int RecordId { get; set; }
            //当CartId被其他用途占用时可以用key指定关键字
            
            public string CartId { get; set; }
            //给每辆购物车唯一的编号

            public int AlbumId { get; set; }
            //对应买的专辑的种类

            public int Count { get; set; }
            //存储购买同一种专辑的数量

            public System.DateTime DateCreated { get; set; }
            //指定时间

            public virtual Album Album { get; set; }
            //通过该对象属性来获取专辑的price
      
    }
}