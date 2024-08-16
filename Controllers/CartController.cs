using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DA_BAN_SACH.Models;

namespace DA_BAN_SACH.Controllers
{
    public class CartController : Controller
    {
        QLBanSachDbcontext context = new QLBanSachDbcontext();
        // Key lưu chuỗi json của Cart
        private const string CARTKEY = "cart";

        public ActionResult Index()
        {
            var cart = Session[CARTKEY] as List<CartItem> ?? new List<CartItem>();
            return View(cart);
        }
            // GET : Product/AddToCart
            public ActionResult AddToCart(int productId)
        {
            // Lấy sản phẩm từ database theo ID
            var product = context.Saches.Find(productId);

            // Nếu sản phẩm không tồn tại
            if (product == null)
            {
                return HttpNotFound();
            }

            // Lấy giỏ hàng từ Session
            var cart = Session[CARTKEY] as List<CartItem> ?? new List<CartItem>();

            // Tìm kiếm trong danh sách giỏ hàng xem có sản phẩm này rồi chưa
            var cartItem = cart.FirstOrDefault(item => item.sach.MaSach == product.MaSach);

            // Nếu sản phẩm đã có trong giỏ hàng trước đó
            if (cartItem != null)
            {
                // Tăng số lượng sản phẩm lên 1
                cartItem.Quantity++;
                // Check if the quantity is zero and remove the item from cart
                if (cartItem.Quantity == 0)
                {
                    cart.Remove(cartItem);
                }
            }
            // Nếu sản phẩm chưa có trong giỏ hàng
            else
            {
                // Thêm sản phẩm vào giỏ hàng
                cart.Add(new CartItem()
                {
                    sach = product,
                    Quantity = 1
                });
            }

            // Lưu giỏ hàng vào Session
            Session[CARTKEY] = cart;

            // Chuyển hướng về trang giỏ hàng
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateCart(FormCollection field)
        {
            string[] quantities = field.GetValues("quantity");
            List<CartItem> ListCart = (List<CartItem>)Session["cart"];
            for (int i = 0; i < ListCart.Count; i++)
            {
                if (Convert.ToInt32(quantities[i]) <= 0)
                {
                    ListCart.RemoveAt(i); // loại bỏ sản phẩm
                }
                else
                {
                    ListCart[i].Quantity = Convert.ToInt32(quantities[i]);
                }

                //ListCart[i].Quantity = Convert.ToInt32(quantities[i]);

            }

            Session["cart"] = ListCart;
            return RedirectToAction("Index");
        }
        private int isExisting(int ?id)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].sach.MaSach == id)
                    return i;
            }
            return -1;
        }
        public ActionResult RemoveItem(int? Id) 
        {
            if (Id == null) 
                { 
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
                }
                int check = isExisting(Id);
                List<CartItem> ListCart = (List<CartItem>)Session["cart"]; 
                ListCart.RemoveAt(check); 
                if (ListCart.Count == 0) 
                { 
                    Session["cart"] = null; 
                } 
                else
                { 
                    Session["cart"] = ListCart;
                } 
                return RedirectToAction("Index"); 
        }
        // GET : Product/Checkout
        public ActionResult Checkout()
        {
            // Lấy giỏ hàng từ Session
            var cart = Session[CARTKEY] as List<CartItem> ?? new List<CartItem>();

            // Nếu giỏ hàng trống
            if (!cart.Any())
            {
                return RedirectToAction("Index");
            }

            // Lưu giỏ hàng vào database

            // Xóa giỏ hàng khỏi Session
            Session.Remove(CARTKEY);

            return RedirectToAction("ThankYou");
        }

        // GET : Product/ThankYou
        public ActionResult ThankYou()
        {
            return View();
        }
   
    
   }
}


