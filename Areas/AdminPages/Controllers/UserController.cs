using DA_BAN_SACH.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DA_BAN_SACH.Areas.AdminPages.Controllers
{
    public class UserController : Controller
    {
        // GET: AdminPages/User
        QLBanSachDbcontext db = new QLBanSachDbcontext();
        [HttpGet]
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var users = new List<KhachHang>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                users = db.KhachHangs.Where(s => s.HoTen.Contains(SearchString)).ToList();
            }
            else
            {
                users = db.KhachHangs.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            users = users.OrderByDescending(n => n.MaKH).ToList();
            return View(users.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(KhachHang users)
        {

            if (ModelState.IsValid)
            {
                users.MatKhau = GetMD5(users.MatKhau);
                db.KhachHangs.Add(users);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(users);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var users = db.KhachHangs.Where(n => n.MaKH == id).FirstOrDefault();

            return View(users);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var users = db.KhachHangs.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var users = db.KhachHangs.Find(id);
            db.KhachHangs.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var users = db.KhachHangs.Where(n => n.MaKH == id).FirstOrDefault();
            return View(users);
        }

        [HttpPost]
        public ActionResult Edit(int id, KhachHang users)
        {
            users.MatKhau = GetMD5(users.MatKhau);
            db.Entry(users).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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