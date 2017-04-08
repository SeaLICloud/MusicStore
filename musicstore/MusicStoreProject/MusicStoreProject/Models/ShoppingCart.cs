using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStoreProject.Models
{
    public class ShoppingCart
    {
        //数据库上下文
        readonly MusicStoreEntities _storeDb = new MusicStoreEntities();
        
        //购物车标识
        string ShoppingCartId { get; set; }
       

        //用于在Session中保存当前用户的购物车标识
        public const string CartSessionKey = "CartId";


        //获取用户的购物车
        public static ShoppingCart GetCart(HttpContextBase context)//参数传入一个网站上下文
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }



        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }


        //在购物车中加入新专辑
        public void AddToCart(Album album)
        {
            // Get the matching cart and album instances
            //检查是否已经在购物车中
            var cartItem = _storeDb.Carts.SingleOrDefault(
            c => c.CartId == ShoppingCartId
            && c.AlbumId == album.AlbumId);
            
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    AlbumId = album.AlbumId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                _storeDb.Carts.Add(cartItem);
            }

            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            //保存到数据库中
            _storeDb.SaveChanges();
        }




        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = _storeDb.Carts.Single(
            cart => cart.CartId == ShoppingCartId
            && cart.RecordId == id);

            var itemCount = 0;
            if (cartItem == null) return itemCount;

            if (cartItem.Count > 1)
            {
                cartItem.Count--;
                itemCount = cartItem.Count;
            }
            else
            {
                _storeDb.Carts.Remove(cartItem);
            }
            // Save changes
            _storeDb.SaveChanges();
            return itemCount;
        }


        //清空购物车
        public void EmptyCart()
        {
            var cartItems = _storeDb.Carts.Where(cart => cart.CartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                _storeDb.Carts.Remove(cartItem);
            }
            // Save changes
            _storeDb.SaveChanges();
        }



        //获取购物车中的购物项
        public List<Cart> GetCartItems()
        {
            return _storeDb.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }


        //获取有多少个项目
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            var count = (from cartItems in _storeDb.Carts
                where cartItems.CartId == ShoppingCartId
                select (int?) cartItems.Count).Sum();//（int?:表示可空）
            // Return 0 if all entries are null
            return count ?? 0;//??判断前面是否为空,如果为空则返回??后面的默认值
        }


        //计算总价
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total

            var total = (from cartItems in _storeDb.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Album.Price).Sum();
            return total ?? decimal.Zero;
        }


        //创建订单
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();
            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderId,//设置所属的订单
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Album.Price);
                _storeDb.OrderDetails.Add(orderDetail);
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;
            // Save the order
            _storeDb.SaveChanges();
            // Empty the shopping cart
            //清空购物车
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }




        // We're using HttpContextBase to allow access to cookies.
        //拿到购物车订单
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                //如果当前没有购物车
                //购物车需要区分
                //如果用户已经登录,那么,用户名就是购物车的标识
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    //为匿名用户创建一个GUID
                    var tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username

        //合并购物车
        //将匿名方式下的购物车合并到某个用户的购物车中
        public void MigrateCart(string userName)
        {
            //找到匿名用户原有的购物项目
            var shoppingCart = _storeDb.Carts.Where(c => c.CartId == ShoppingCartId);
           
            //遍历所有的购物项目,将购物项目的所有人修改为当前登录的用户
            foreach (var item in shoppingCart)
            {
                item.CartId = userName;
            }
            _storeDb.SaveChanges();
        }
    }
}

