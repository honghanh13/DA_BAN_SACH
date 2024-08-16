using DA_BAN_SACH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_BAN_SACH.Controllers
{
    public class Check_outController : Controller
    {
        QLBanSachDbcontext context = new QLBanSachDbcontext();
        

        // GET: Check_out
        public ActionResult Index()
        {
            List<CartItem> ListCart = (List<CartItem>)Session["cart"];
            
            return View(ListCart);
        }

       
        [HttpPost]
        public ActionResult Processorder(FormCollection field)
        {
            List<CartItem> ListCart = (List<CartItem>)Session["cart"];
            //1. Save the order into Order table//
            var order = new DA_BAN_SACH.Models.DonDatHang()
            {
                Hoten = field["cusName"],
                Sdt = field["cusPhone"],
                Email = field["cusEmail"],
                DiaChi = field["cusAddress"],
                GhiChu = field["Note"],
                NgayDH = DateTime.Now,
                TenDH = "DonHang-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                NgayGiao = DateTime.Now.AddDays(2),
                DaThanhToan = false,
                TinhTrangGiaoHang = false 
            };
            context.DonDatHangs.Add(order);
            context.SaveChanges();
            /*            2.Save the order detail into Order Detail table*/
            foreach (CartItem cart in ListCart)
            {
                ChiTietDatHang orderDetail = new ChiTietDatHang()
                {
                    SoDH = order.SoDH,
                    MaSach = cart.sach.MaSach,
                    SoLuong = Convert.ToInt32(cart.Quantity),
                    DonGia = (decimal?)Convert.ToDouble(cart.sach.GiaBan),
                    TongTien = cart.GetTotalPrice()
                };
                
                context.ChiTietDatHangs.Add(orderDetail);
                context.SaveChanges();
            }
            //3. Remove shopping cart session
            Session.Remove("cart"); 
            return View("OrderSuccess");
        }
        public ActionResult OrderSuccess()
        {
            return View();
        }

    }

       
}
    
