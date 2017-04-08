using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MusicStoreProject.Models
{
//    public class Album//专辑
//    {
//        //类名加上Id的字段在EF创建数据库的时候会自动标记为主键
//        //且其为int型自动成为标识列
//        public int AlbumId { get; set; }
//
//        public int GenreId { get; set; }
//        public int ArtistId { get; set; }
//        public string Title { get; set; }
//        public decimal Price { get; set; }//价格一般用decimal
//        public string AlbumArtUrl { get; set; }//专辑的图片存放路径
//
//        public Genre Genre { get; set; }
//        public Artist Artist { get; set; }
//        //Artist也是一个表,在Album里面有个Artist属性，两个表之间会由EF自动创建主外键关联

    //模型绑定
    [Bind(Exclude = "AlbumId")]//表示不考虑专辑id属性
    public class Album
    {
        [ScaffoldColumn(false)]//支架在生成视图的时候不需要考虑在内
        public int AlbumId { get; set; }

        [DisplayName("Genre")]
        public int GenreId { get; set; }

        [DisplayName("Artist")]
        public int ArtistId { get; set; }

        [Required(ErrorMessage = "An Album Title is required")]
        [StringLength(160)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 100.00,ErrorMessage = "Price must be between 0.01 and 100.00")]
        public decimal Price { get; set; }

        [DisplayName("Album Art URL")]
        [StringLength(1024)]
        public string AlbumArtUrl { get; set; }


        public virtual Genre Genre { get; set; }
        public virtual Artist Artist { get; set; }

        //用来访问实际的销售请情况
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
    
    
//Required 必须 – 表示这个属性是必须提供内容的字段
//DisplayName 显示名 – 定义表单字段的提示名称
//StringLength 字符串长度 – 定义字符串类型的属性的最大长度
//Range 范围 – 为数字类型的属性提供最大值和最小值
//Bind 绑定 – 列出在将请求参数绑定到模型的时候，包含和不包含的字段
//ScaffoldColumn 支架列 - 在编辑表单的时候，需要隐藏起来的的字符   
  }