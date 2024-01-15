using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebBanHoa.Models;
using WebBanHoa.Models.EF;

namespace WebBanHoa.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index(int? page)
        {
            var pageSize = 8;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.CreatedDate);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            return View(items);
        }

        public ActionResult Detail(string alias,int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                db.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            var countReview = db.reviewProducts.Where(x => x.ProductId == id ).Count();
            ViewBag.Count = countReview;

            
            var reviews = db.reviewProducts.Where(x => x.ProductId == id).ToList();
            if (reviews.Any())
            {
                var averageRating = db.reviewProducts.Where(x => x.ProductId == id).Average(x => x.Rate);
                ViewBag.AverageRatingHtml = Common.Common.HtmlRate((int)Math.Round(averageRating));
            }
            else
            {
                ViewBag.AverageRatingHtml = Common.Common.HtmlRate(0);
            }
            return View(item);
        }

        public ActionResult ProductCategory(string alias, int id, int? page)
        {
            var pageSize = 10;
            var pageIndex = page ?? 1;

            var items = db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }

            var cate = db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }

            ViewBag.CateId = id;

            var pagedList = items.ToPagedList(pageIndex, pageSize);
            return View(pagedList);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x => x.IsHome && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductSales()
        {
            var items = db.Products.Where(x => x.IsSale && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }
    }
}