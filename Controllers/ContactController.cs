using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_BAN_SACH.Models;

namespace DA_BAN_SACH.Controllers
{
    public class ContactController : Controller
    {
        QLBanSachDbcontext db = new QLBanSachDbcontext();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Index(Contact contact)
        {
            if (ModelState.IsValid)
            {

                contact.NgayGui = DateTime.Now;
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            var errorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
            ViewBag.ErrorMessages = errorMessages;

            return View(contact);
        }


       
    }
}