using DA_BAN_SACH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Security;
using System.Text;

namespace DA_BAN_SACH.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QLBanSachDbcontext context = new QLBanSachDbcontext();
        public ActionResult Index()
        {
            var books = context.Saches.ToList();
            var baiViet = context.BaiViets.Where(x => x.DaDuyet == true).ToList();
            var ramdomBooks = context.Saches.OrderBy(x => Guid.NewGuid()).Take(4).Distinct().ToList();
            var popularBooks = context.Saches
                .Where(x => x.Popular == true).OrderBy(x => Guid.NewGuid()).Take(8).Distinct().ToList();
            var bookDeals = context.Saches
                .Where(x => x.BestSellers == true).OrderBy(x => Guid.NewGuid()).Take(8) .Distinct().ToList();
            ViewBag.rambooks = ramdomBooks;
            ViewBag.popBooks = popularBooks;
            ViewBag.Deals = bookDeals;
            var model = new SachBaiViet
            {
                Saches = books,
                BaiViets = baiViet
            };
            return View(model);
        
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(KhachHang kh)
        {
           
                kh.MatKhau = GetMD5(kh.MatKhau);
                bool IsValidKhachHang = context.KhachHangs.Any(khachhang => khachhang.TaiKhoan.ToLower() ==
                    kh.TaiKhoan.ToLower() && khachhang.MatKhau == khachhang.MatKhau);
                var data = context.KhachHangs.Where(s => s.TaiKhoan.Equals(kh.TaiKhoan) && s.MatKhau.Equals(kh.MatKhau)).ToList();
                if (IsValidKhachHang)
                {
                    FormsAuthentication.SetAuthCookie(kh.TaiKhoan, false);
                    Session["Hoten"] = data.FirstOrDefault().HoTen;
                    Session["TaiKhoan"] = data.FirstOrDefault().TaiKhoan;
                    Session["MaKH"] = data.FirstOrDefault().MaKH;

                    return RedirectToAction("Index", "Shop");
                }
            ViewBag.errorlogin = "Tài khoản hoặc mật khẩu không đúng";
            return View();
            
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(KhachHang kh)
        {
            bool isDuplicate = context.KhachHangs.Any(x => x.Email == kh.Email || x.TaiKhoan == kh.TaiKhoan);
            if (isDuplicate)
            {
                ViewBag.error = "Tài khoản hoặc email đã tồn tại trong hệ thống";
                return View("Login");
            }

            kh.MatKhau = GetMD5(kh.MatKhau);
            context.KhachHangs.Add(kh);
            context.SaveChanges();

            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i=0; i<targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;

        }
    }
}
  