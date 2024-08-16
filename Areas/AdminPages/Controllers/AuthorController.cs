using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DA_BAN_SACH.Models;
using PagedList;

namespace DA_BAN_SACH.Areas.AdminPages.Controllers
{
    public class AuthorController : Controller
    {
        QLBanSachDbcontext db = new QLBanSachDbcontext();
        [HttpGet]
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var tacGia = new List<TacGia>();
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
                tacGia = db.TacGias.Where(s => s.TenTG.Contains(SearchString)).ToList();
            }
            else
            {
                tacGia = db.TacGias.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            tacGia = tacGia.OrderByDescending(n => n.MaTG).ToList();
            return View(tacGia.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TacGia tacGia)
        {

            if (ModelState.IsValid)
            {
               
                db.TacGias.Add(tacGia);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(tacGia);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var tacGia = db.TacGias.Where(n => n.MaTG == id).FirstOrDefault();

            return View(tacGia);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tacGia = db.TacGias.Find(id);
            if (tacGia == null)
            {
                return HttpNotFound();
            }
            return View(tacGia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var tacGia = db.TacGias.Find(id);
            db.TacGias.Remove(tacGia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var tacGia = db.TacGias.Where(n => n.MaTG == id).FirstOrDefault();
            return View(tacGia);
        }

        [HttpPost]
        public ActionResult Edit(int id, TacGia tacGia)
        {
            db.Entry(tacGia).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}