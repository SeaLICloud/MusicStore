namespace MusicStoreProject.Models
{
    public class Artist //艺术家
    {
        //类名加上Id的字段在EF创建数据库的时候会自动标记为主键
        //且其为int型自动成为标识列
        public int ArtistId { get; set; }
        public string Name { get; set; }
    }
}