using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_BAN_SACH.Models;

namespace DA_BAN_SACH.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        QLBanSachDbcontext db = new QLBanSachDbcontext();
        public ActionResult Index(int? maCD, int? maNXB, string searchString)
        {
            var sachs = db.Saches.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                sachs = sachs.Where(s => s.TenSach.ToLower().Contains(searchString.ToLower())).ToList();
            }
            if (maCD.HasValue)
            {
                sachs = sachs.Where(s => s.MaCD == maCD).ToList();
            }
            if (maNXB.HasValue)
            {
                sachs = sachs.Where(s => s.MaNXB == maNXB).ToList();
            }
            var nxb = db.NhaXuatBans.ToList();
            var chuDe = db.ChuDes.ToList();
            ViewBag.ChuDe = new SelectList(chuDe, "MaCD", "TenChuDe", maCD);
            ViewBag.NhaXuatBan = new SelectList(nxb, "MaNXB", "TenNXB", maNXB);

            return View(sachs);
        }
        public ActionResult ShopDetails(int id)
        {
            
            Sach Sachs = db.Saches.FirstOrDefault(x => x.MaSach == id);
            return View(Sachs);
        }
    }
}