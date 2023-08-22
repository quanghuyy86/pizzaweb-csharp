using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using pizzaweb.Infrastructure;
using pizzaweb.Models;
using pizzaweb.Models.Tables;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace pizzaweb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Register(TblCustomer customer)
        {
            pizzaweb.Models.Tables.pizzawebContext db = new pizzawebContext();
            if (ModelState.IsValid)
            {
                var check = db.TblCustomers.FirstOrDefault(s => s.Email == customer.Email);
                if (check == null)
                {
                    customer.Password = GetMD5(customer.Password);
              
                    db.TblCustomers.Add(customer);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    TempData["error"] = "Email đã tồn tại";
                    return View();
                }


            }
            return View();

        }

        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            pizzaweb.Models.Tables.pizzawebContext db = new pizzawebContext();

            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var count = db.TblUsers.Where(m => m.Username.Equals(email)  && m.Password.Equals(f_password)).ToList();
                var data = db.TblCustomers.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (count.Count() >0)
                {
                    HttpContext.Session.SetJson("user", email);
                    ViewBag.user = email;
                    return RedirectToAction("listProducts", "admin");
                }
                else if (data.Count() > 0)
                {
                    

                    HttpContext.Session.SetJson("FullName", data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName);
                    HttpContext.Session.SetJson("Email", data.FirstOrDefault().Email);
                    HttpContext.Session.SetJson("idCustomers", data.FirstOrDefault().Id);

                    return RedirectToAction("Pizza", "Home");
                }
                else
                {
                    TempData["error"] = "Tài khoản đăng nhập không đúng";
                    return View();
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login", "home");
        }

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(TblContact tblContact) 
        {
            //Thêm mới vào bản ghi
            pizzaweb.Models.Tables.pizzawebContext db = new pizzaweb.Models.Tables.pizzawebContext();
            db.TblContacts.Add(tblContact);
            //Lưu lại thay đổi
            db.SaveChanges();

            return RedirectToAction("pizza");
        }

        public IActionResult Pizza()
        {
            pizzaweb.Models.Tables.pizzawebContext db = new pizzawebContext();
            var categoryId = 1; 
            var products = db.TblProducts.Where(p => p.CategoryId == categoryId).ToList();
            return View(products);
        }

        public IActionResult spaghetti()
        {
            pizzaweb.Models.Tables.pizzawebContext db = new pizzawebContext();
            var categoryId = 2;
            var products = db.TblProducts.Where(p => p.CategoryId == categoryId).ToList();
            return View(products);
        }
        public IActionResult Bbq()
        {
            pizzaweb.Models.Tables.pizzawebContext db = new pizzawebContext();
            var categoryId = 3;
            var products = db.TblProducts.Where(p => p.CategoryId == categoryId).ToList();
            return View(products);
        }

      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}