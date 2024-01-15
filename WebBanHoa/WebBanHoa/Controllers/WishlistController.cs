using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHoa.Models;
using WebBanHoa.Models.EF;
using System.Web.UI;
using PagedList;

namespace WebBanHoa.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        // GET: Wishlist
        public ActionResult Index(int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Wishlist> items = db.Wishlists.Where(x => x.UserName == User.Identity.Name).OrderByDescending(x => x.CreatedDate);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(items);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PostWishlist(int ProductId)
        {
            if (Request.IsAuthenticated == false)
            {
                return Json (new { Success = false, Message = "Bạn chưa đăng nhập!!!" });
            }
            var checkItem = db.Wishlists.FirstOrDefault(x => x.ProductId == ProductId && x.UserName == User.Identity.Name);
            if (checkItem != null)
            {
                return Json(new { Success = false, Message = "Sản phẩm đã được thêm vào yêu thích"});
            }
            var item = new Wishlist();
            item.ProductId = ProductId;
            item.UserName = User.Identity.Name;
            item.CreatedDate = DateTime.Now;
            db.Wishlists.Add(item);
            db.SaveChanges();
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult RemoveFromWishlist(int id)
        {
            var wishlistItem = db.Wishlists.Find(id);
            if (wishlistItem != null)
            {
                db.Wishlists.Remove(wishlistItem);
                db.SaveChanges();
                return Json(new { Success = true, Message = "Sản phẩm đã được xóa khỏi danh sách yêu thích" });
            }
            return Json(new { Success = false, Message = "Không tìm thấy sản phẩm trong danh sách yêu thích" });
        }

        [HttpPost]
        public ActionResult PostDeleteWishlist(int ProductId)
        {
            var checkItem = db.Wishlists.FirstOrDefault(x => x.ProductId == ProductId && x.UserName == User.Identity.Name);
            if (checkItem != null)
            {
                db.Wishlists.Remove(checkItem);
                db.SaveChanges();
                return Json(new { Success = true, Message = "Xóa thành công!" });
            }
            return Json(new { Success = false, Message = "Xóa thất bại!!!" });
        }

        private ApplicationDbContext db = new ApplicationDbContext();
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}