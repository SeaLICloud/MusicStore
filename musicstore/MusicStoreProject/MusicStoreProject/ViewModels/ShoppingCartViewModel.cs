using System.Collections.Generic;
using MusicStoreProject.Models;

namespace MusicStoreProject.ViewModels
{
    public class ShoppingCartViewModel
    {
        //购物项目集合
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }

    }
}