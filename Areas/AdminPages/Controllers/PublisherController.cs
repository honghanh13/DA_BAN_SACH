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
    public class PublisherController : Controller
    {
        // GET: AdminPages/Publisher
        QLBanSachDbcontext db = new QLBanSachDbcontext();
        [HttpGet]
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var nxb = new List<NhaXuatBan>();
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
                nxb = db.NhaXuatBans.Where(s => s.TenNXB.Contains(SearchString)).ToList();
            }
            else
            {
                nxb = db.NhaXuatBans.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            nxb = nxb.OrderByDescending(n => n.MaNXB).ToList();
            return View(nxb.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(NhaXuatBan nxb)
        {

            if (ModelState.IsValid)
            {

                db.NhaXuatBans.Add(nxb);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(nxb);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var nxb = db.NhaXuatBans.Where(n => n.MaNXB == id).FirstOrDefault();

            return View(nxb);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var nxb = db.NhaXuatBans.Find(id);
            if (nxb == null)
            {
                return HttpNotFound();
            }
            return View(nxb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var nxb = db.NhaXuatBans.Find(id);
            db.NhaXuatBans.Remove(nxb);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var nxb = db.NhaXuatBans.Where(n => n.MaNXB == id).FirstOrDefault();
            return View(nxb);
        }

        [HttpPost]
        public ActionResult Edit(int id, NhaXuatBan nxb)
        {
            db.Entry(nxb).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}