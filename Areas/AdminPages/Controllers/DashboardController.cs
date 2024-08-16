using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DA_BAN_SACH.Models;

namespace DA_BAN_SACH.Areas.AdminPages.Controllers
{
    public class DashboardController : Controller
    {
        // GET: AdminPages/Dashboard
        QLBanSachDbcontext context = new QLBanSachDbcontext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin admin)
        {

                admin.PassAdmin = GetMD5(admin.PassAdmin);
                bool IsValidKhachHang = context.Admins.Any(x => x.UserAdmin.ToLower() ==
                    admin.UserAdmin.ToLower() && x.PassAdmin == admin.PassAdmin);
                var data = context.Admins.Where(s => s.UserAdmin.Equals(admin.UserAdmin) && s.PassAdmin.Equals(admin.PassAdmin)).ToList();
                if (IsValidKhachHang)
                {
                    FormsAuthentication.SetAuthCookie(admin.UserAdmin, false);
                    Session["Hoten"] = data.FirstOrDefault().Hoten;
                    Session["TaiKhoan"] = data.FirstOrDefault().UserAdmin;
               

                    return RedirectToAction("Index", "NewProducts");
                }
                ModelState.AddModelError("", "invalid TaiKhoan or MatKhau");
                return View();
           
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
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;

        }
    }
}