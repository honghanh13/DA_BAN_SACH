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
    public class NewProductsController : Controller
    {
        QLBanSachDbcontext db = new QLBanSachDbcontext();
        [HttpGet]
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstProduct = new List<Sach>();
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
                lstProduct = db.Saches.Where(s => s.TenSach.Contains(SearchString)).ToList();
            }
            else
            {
                lstProduct = db.Saches.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            lstProduct = lstProduct.OrderByDescending(n => n.MaSach).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(db.ChuDes, "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB");
            ViewBag.MaTG = new SelectList(db.TacGias, "MaTG", "TenTG");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Sach objproduct)
        {
            try
            {
                if (objproduct.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objproduct.ImageUpload.FileName);
                    string extension = Path.GetExtension(objproduct.ImageUpload.FileName);
                    fileName = fileName + extension;
                    objproduct.AnhBia = fileName;
                    objproduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/"), fileName));

                }
                objproduct.NgayCapNhat = DateTime.Now;
                db.Saches.Add(objproduct);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");

            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objProduct = db.Saches.Where(n => n.MaSach == id).FirstOrDefault();

            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var objPro = db.Saches.Find(id);
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
            var objPro = db.Saches.Find(id);
            db.Saches.Remove(objPro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach Sach = db.Saches.Find(id);
            if (Sach == null)
            {
                return HttpNotFound();
            }

            // Load các giá trị cho SelectList
            ViewBag.MaCD = new SelectList(db.ChuDes, "MaCD", "TenChuDe", Sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", Sach.MaNXB);
            ViewBag.MaTG = new SelectList(db.TacGias, "MaTG", "TenTG", Sach.MaTG);

            return View(Sach);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, Sach sach, HttpPostedFileBase ImageUpload)
        {
            if (ModelState.IsValid)
            {
                // Nếu người dùng đã chọn một tệp ảnh để cập nhật
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    // Xóa tệp ảnh cũ
                    if (sach.AnhBia != null)
                    {
                        var fullPath = Path.Combine(Server.MapPath("~/Images/"), sach.AnhBia);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    string fileName = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
                    string extension = Path.GetExtension(ImageUpload.FileName);
                    fileName = fileName + extension;
                    sach.AnhBia = fileName;
                    ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/"), fileName));
                }
                else
                {
                    // Lấy giá trị của trường ẩn OldAnhBia nếu có
                    var oldAnhBia = Request["OldAnhBia"];
                    if (!String.IsNullOrEmpty(oldAnhBia))
                    {
                        sach.AnhBia = oldAnhBia;
                    }
                }
                sach.NgayCapNhat = DateTime.Now;
                // Cập nhật đối tượng Sach trong cơ sở dữ liệu
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();

                // Chuyển hướng người dùng đến trang Index
                return RedirectToAction("Index");
            }

            // Load lại các giá trị SelectList
            ViewBag.MaCD = new SelectList(db.ChuDes, "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sach.MaNXB);
            ViewBag.MaTG = new SelectList(db.TacGias, "MaTG", "TenTG", sach.MaTG);

            return View(sach);
        }




    }
}