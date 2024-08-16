using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DA_BAN_SACH.Models;
using PagedList;

namespace DA_BAN_SACH.Areas.AdminPages.Controllers
{
    public class ContactsController : Controller
    {
        QLBanSachDbcontext db = new QLBanSachDbcontext();
        [HttpGet]
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstProduct = new List<Contact>();
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
                lstProduct = db.Contacts.Where(s => s.HoTen.Contains(SearchString)).ToList();
            }
            else
            {
                lstProduct = db.Contacts.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            lstProduct = lstProduct.OrderByDescending(n => n.IdContact).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }


        

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objProduct = db.Contacts.Where(n => n.IdContact == id).FirstOrDefault();

            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var objPro = db.Contacts.Find(id);
            if (objPro == null)
            {
                return HttpNotFound();
            }
            return View(objPro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var objPro = db.Contacts.Find(id);
            db.Contacts.Remove(objPro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objProduct = db.Contacts.Where(n => n.IdContact == id).FirstOrDefault();
            
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Edit(int id, Contact objProduct)
        {
            
            db.Entry(objProduct).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}