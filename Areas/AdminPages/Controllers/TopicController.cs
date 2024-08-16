using DA_BAN_SACH.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DA_BAN_SACH.Areas.AdminPages.Controllers
{
    public class TopicController : Controller
    {
        // GET: AdminPages/Topic
        QLBanSachDbcontext db = new QLBanSachDbcontext();
        [HttpGet]
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var chuDes = new List<ChuDe>();
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
                chuDes = db.ChuDes.Where(s => s.TenChuDe.Contains(SearchString)).ToList();
            }
            else
            {
                chuDes = db.ChuDes.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            chuDes = chuDes.OrderByDescending(n => n.MaCD).ToList();
            return View(chuDes.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ChuDe chuDes)
        {

            if (ModelState.IsValid)
            {

                db.ChuDes.Add(chuDes);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(chuDes);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var chuDes = db.ChuDes.Where(n => n.MaCD == id).FirstOrDefault();

            return View(chuDes);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var chuDes = db.ChuDes.Find(id);
            if (chuDes == null)
            {
                return HttpNotFound();
            }
            return View(chuDes);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var chuDes = db.ChuDes.Find(id);
            db.ChuDes.Remove(chuDes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var chuDes = db.ChuDes.Where(n => n.MaCD == id).FirstOrDefault();
            return View(chuDes);
        }

        [HttpPost]
        public ActionResult Edit(int id, ChuDe chuDes)
        {
            db.Entry(chuDes).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}