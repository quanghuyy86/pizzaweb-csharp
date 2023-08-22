using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using pizzaweb.Models.Tables;
using System.Drawing.Printing;
using System.Security.Cryptography;
using System.Text;
using X.PagedList;

namespace pizzaweb.Controllers
{
    public class AdminController : Controller
    {
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

        public IActionResult Home()
        {
            if (HttpContext.Session.GetString("user") != null)
			{
				return View();
			}
			else
			{
				// Chuyển hướng người dùng về trang đăng nhập
				return RedirectToAction("Login", "Home");
			}
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string user, string password)
        {
            //check db
            pizzaweb.Models.Tables.pizzawebContext db = new pizzawebContext();
            int count = db.TblUsers.Count(m => m.Username == user && m.Password == password);
            //check code
            if (count == 1)
            {
                HttpContext.Session.SetString("user", user); 
                ViewBag.user = user;
                return RedirectToAction("listProducts");
            }
            else
            {
                TempData["error"] = "Tài khoản đăng nhập không đúng";
                return View();
            }
        }

        public IActionResult Logout()
        {

            HttpContext.Session.Remove("user");
            
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Contact()
        {

            if (HttpContext.Session.GetString("user") != null)
            {
                pizzaweb.Models.Tables.pizzawebContext db = new pizzawebContext();
                var contact = db.TblContacts.ToList();

                return View(contact);
            }
            else
            {
                // Chuyển hướng người dùng về trang đăng nhập
                return RedirectToAction("Login", "Home");
            }

        }

        public IActionResult Order() 
        {

            if (HttpContext.Session.GetString("user") != null)
            {
                pizzawebContext db = new pizzawebContext();
                var other = db.TblOrders.ToList();
                return View(other);
            }
            else
            {
                // Chuyển hướng người dùng về trang đăng nhập
                return RedirectToAction("Login", "Home");
            }
            
        }

        public IActionResult ListAdmin()
        {
            
            if (HttpContext.Session.GetString("user") != null)
            {
                pizzawebContext db = new pizzawebContext();
                var admin = db.TblUsers.ToList();
                return View(admin);
            }
            else
            {
                // Chuyển hướng người dùng về trang đăng nhập
                return RedirectToAction("Login", "Home");
            }


        }


        public IActionResult CreateAdmin()
        {
            
            if (HttpContext.Session.GetString("user") != null)
            {
                return View();
            }
            else
            {
                // Chuyển hướng người dùng về trang đăng nhập
                return RedirectToAction("Login", "Home");
            }

        }

        [HttpPost]
        public IActionResult CreateAdmin(TblUser user)
        {

            pizzaweb.Models.Tables.pizzawebContext db = new pizzawebContext();
            if (ModelState.IsValid)
            {
                var check = db.TblUsers.FirstOrDefault(s => s.Email == user.Email);
                if (check == null)
                {
                    user.Password = GetMD5(user.Password);

                    db.TblUsers.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("listadmin", "admin");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }

            }
            return View();


        }

        public IActionResult DeleteAdmin(int id)
        {
            pizzawebContext db = new pizzawebContext();
            var deleteModel = db.TblUsers.Find(id);
            db.TblUsers.Remove(deleteModel);
            db.SaveChanges();

            return RedirectToAction("listadmin", "admin");
        }

        public IActionResult ListProducts(string keyword, int page = 1)
        {


            if (HttpContext.Session.GetString("user") != null)
            {
                pizzawebContext db = new pizzawebContext();
                int pageSize = 6;

                var search = from b in db.TblProducts select b;

                if (!string.IsNullOrEmpty(keyword))
                {
                    search = search.Where(x => x.Title.Contains(keyword));

                }
                //List<TblProduct> tblProducts = db.TblProducts.Where(m => m.Title.ToLower().Contains(keyword) == true).ToList();

                search = search.OrderBy(x => x.Title);

                var pagedList = search.ToPagedList(page, pageSize);
                ViewBag.Keyword = keyword;
                ViewBag.PageNumber = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItemCount = pagedList.TotalItemCount;
                ViewBag.TotalPageCount = pagedList.PageCount;


                return View(pagedList);
            }
            else
            {
                // Chuyển hướng người dùng về trang đăng nhập
                return RedirectToAction("Login", "Home");
            }
        }

        public IActionResult CreateProduct() 
        {
            
            if (HttpContext.Session.GetString("user") != null)
            {
                return View();
            }
            else
            {
                // Chuyển hướng người dùng về trang đăng nhập
                return RedirectToAction("Login", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(TblProduct tblProduct, IFormFile avatar) 
        {
            if(avatar.Length > 0)
            {
                //Lưu file
                string currentDirectory = Directory.GetCurrentDirectory();
                string uploadsFolderPath = Path.Combine(currentDirectory, "wwwroot/uploads/");
                string fileNameAndPath = uploadsFolderPath + avatar.FileName;
                using (var fileStream = new FileStream(fileNameAndPath, FileMode.Create))
                {
                    await avatar.CopyToAsync(fileStream);
                }

                //Lưu vào database
                tblProduct.Avatar = "/uploads/" + avatar.FileName;

            }
           
            //Thêm mới bản ghi
            pizzaweb.Models.Tables.pizzawebContext db = new pizzaweb.Models.Tables.pizzawebContext();
            db.TblProducts.Add(tblProduct);
            //Lưu lại thay đổi
            db.SaveChanges();

            return RedirectToAction("ListProducts");
        }


        

        public IActionResult EditProduct(int id)
        {
            //Tìm đối tượng theo ID
            pizzawebContext db = new pizzawebContext();
            TblProduct tblProduct = db.TblProducts.Find(id);
            return View(tblProduct);

        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(TblProduct tblProduct, IFormFile avatar) 
        {
            pizzawebContext db = new pizzawebContext();
            //tìm đối tượng 
            var updateModel = db.TblProducts.Find(tblProduct.Id);
            //gán giá trị
            updateModel.Title = tblProduct.Title;   
            updateModel.Price = tblProduct.Price;
            updateModel.PriceSale = tblProduct.PriceSale;
            updateModel.DetailDescription = tblProduct.DetailDescription;
            updateModel.CategoryId = tblProduct.CategoryId;

            if (avatar.Length > 0)
            {

                // Xóa ảnh cũ
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", avatar.FileName);
                System.IO.File.Delete(imagePath);

                //Lưu file
                string currentDirectory = Directory.GetCurrentDirectory();
                string uploadsFolderPath = Path.Combine(currentDirectory, "wwwroot/uploads/");
                string fileNameAndPath = uploadsFolderPath + avatar.FileName;
                using (var fileStream = new FileStream(fileNameAndPath, FileMode.Create))
                {
                    await avatar.CopyToAsync(fileStream);
                }

                updateModel.Avatar = Path.Combine("/uploads/" + avatar.FileName);

                //Lưu vào database
                //tblProduct.Avatar = "/uploads/" + avatar.FileName;
                await db.SaveChangesAsync();
            }

            //Lưu thay đổi
            db.SaveChanges();
            return RedirectToAction("ListProducts");
        }

        public IActionResult DeleteProduct(int id)
        {
            pizzawebContext db = new pizzawebContext();
            var deleteModel = db.TblProducts.Find(id);
            db.TblProducts.Remove(deleteModel);
            db.SaveChanges();

            return RedirectToAction("ListProducts");
        }
    }
}
