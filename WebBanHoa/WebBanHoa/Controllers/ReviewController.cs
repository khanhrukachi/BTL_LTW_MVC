using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHoa.Models;
using WebBanHoa.Models.EF;

namespace WebBanHoa.Controllers
{
    public class ReviewController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Review
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult _Review(int productId)
        {
            ViewBag.ProductId = productId;
            var item = new ReviewProduct();
            if (User.Identity.IsAuthenticated) 
            {
                var userStore =  new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);
                if (user != null)
                {
                    item.UserName = user.UserName;
                    item.Email = user.Email;
                    item.FullName = user.FullName;
                }
                return PartialView(item);
            }
            return PartialView();
        }

        public ActionResult OrderHistory()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);

                // Kiểm tra xem người dùng có đơn hàng không
                var hasOrders = _db.Orders.Any(x => x.CustomerId == user.Id);

                if (hasOrders)
                {
                    // Lấy danh sách các đơn hàng của người dùng
                    var items = _db.Orders.Where(x => x.CustomerId == user.Id).ToList();
                    return PartialView(items);
                }
            }

            // Người dùng chưa xác thực hoặc không có đơn hàng
            return PartialView();
        }

        public ActionResult DetailOrderHistory(int id)
        {
            var item = _db.Orders.Find(id);
            return View(item);
        }

        public ActionResult Partial_SanPham(int id)
        {
            var items = _db.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(items);
        }


        public ActionResult _Load_Review(int productId)
        {
            var item = _db.reviewProducts.Where(x => x.ProductId == productId).OrderByDescending(x=>x.Id).ToList();
            ViewBag.Count = item.Count; 
            return PartialView(item);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult PostReview(ReviewProduct reg)
        {
            if (ModelState.IsValid)
            {
                reg.CreatedDate = DateTime.Now;
                _db.reviewProducts.Add(reg);
                _db.SaveChanges();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}