using System;
using System.Linq;
using System.Web.Mvc;
using MusicStoreProject.Models;

namespace MusicStoreProject.Controllers
{

    [Authorize]//授权
    //结账的控制器
    public class CheckoutController : Controller
    {
        //数据库的访问上下文
        readonly MusicStoreEntities _storeDb = new MusicStoreEntities();
        //促销码
        const string PromoCode = "FREE";
        //结账的控制器
        //检查用户是否已经登录

        //显示结账表单
        public ActionResult AddressAndPayment()
        {
            return View();
        }


        //获取用户数据
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            //生成订单对象
            var order = new Order();
            //通过模型绑定，将请求参数绑定到模型对象上
            TryUpdateModel(order);

            try
            {
                //检查促销码是否是对的
                if (!string.Equals(values["PromoCode"], PromoCode,
                StringComparison.OrdinalIgnoreCase))
                //第三个参数忽略大小写
                {
                    return View(order);
                }

                else
                {
                    //填对了生成订单
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;
                    //Save Order
                    //保存订单
                    //保存之后获得订单编号
                    //通过订单编号保存订单明细
                    _storeDb.Orders.Add(order);
                    _storeDb.SaveChanges();

                    //Process the order
                    //保存订单的明细
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);
                    return RedirectToAction("Complete",new { id = order.OrderId });
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }

        }


        //完成订单之后的提示
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            var isValid = _storeDb.Orders.Any(
            o => o.OrderId == id &&
            o.Username == User.Identity.Name);
            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }


    }
}
