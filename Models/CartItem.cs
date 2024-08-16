using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DA_BAN_SACH.Models
{
    public class CartItem
    {
        public int Quantity { set; get; }
        public Sach sach { set; get; }

        public int GetTotalPrice()
        {
            var totalPrice = (int)sach.GiaBan * Quantity;
            if (sach.GiamGia != null && sach.GiamGia > 0)
            {
                totalPrice = (int)(totalPrice * (1 - sach.GiamGia / 100));
            }
            return totalPrice;
        }
    }




}