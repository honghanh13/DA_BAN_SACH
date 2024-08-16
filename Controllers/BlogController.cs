using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_BAN_SACH.Models;

namespace DA_BAN_SACH.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        QLBanSachDbcontext context = new QLBanSachDbcontext();
        public ActionResult Index()
        {
            var list = context.BaiViets.Where(x => x.DaDuyet == true).OrderByDescending(x => x.NgayDang).ToList();
            return View(list);
        }
        public ActionResult blogDetail(int id)
        {
            var baiViet = context.BaiViets.Find(id);
            return View(baiViet);
        }
    }
}