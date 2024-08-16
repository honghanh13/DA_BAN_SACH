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
    public class OrderController : Controller
    {
        // GET: AdminPages/Order
        QLBanSachDbcontext db = new QLBanSachDbcontext();
        [HttpGet]
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var order = new List<DonDatHang>();
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
                order = db.DonDatHangs.Where(s => s.TenDH.Contains(SearchString)).ToList();
            }
            else
            {
                order = db.DonDatHangs.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            order = order.OrderByDescending(n => n.SoDH).ToList();
            return View(order.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(DonDatHang order)
        {

            if (ModelState.IsValid)
            {

                db.DonDatHangs.Add(order);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(order);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var order = db.DonDatHangs.Where(n => n.SoDH == id).FirstOrDefault();

            return View(order);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.DonDatHangs.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = db.DonDatHangs.Find(id);
            db.DonDatHangs.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var order = db.DonDatHangs.Where(n => n.SoDH == id).FirstOrDefault();
            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(int id, DonDatHang order)
        {
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}