using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testApp_Web.DAL;

namespace testApp_Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SkuCategoryDAL cat_Dal = new SkuCategoryDAL();
            List<string> categories = cat_Dal.GetSkuCategoryList(87);

            UserDAL u_Dal = new UserDAL();
            List<string> users = u_Dal.Get_WarehouseUsers();


            ViewBag.Categories = categories;
            ViewBag.User = users;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}