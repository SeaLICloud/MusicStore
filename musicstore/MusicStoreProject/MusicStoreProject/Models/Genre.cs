using System.Collections.Generic;

namespace MusicStoreProject.Models
{
    public class Genre//流派
    {
        //类名加上Id的字段在EF创建数据库的时候会自动标记为主键
        //且其为int型自动成为标识列
        public int GenreId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        //一个集合,当然也是主外键关联
        //Albums:集合属性一般用复数表示
        public List<Album> Albums { get; set; }
    }
}