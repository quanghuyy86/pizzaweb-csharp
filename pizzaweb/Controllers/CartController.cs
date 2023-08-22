using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using pizzaweb.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pizzaweb.Models.Tables;
using pizzaweb.Infrastructure;
using System.IO.MemoryMappedFiles;
using NuGet.Protocol;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using Newtonsoft.Json;

namespace pizzaweb.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetJson<ShopingCart>("Cart");
            if (cart != null)
            {
                return View(cart.Items);
            }

            return View();
        }

        public IActionResult Succsess()
        {
            return View();
        }

        public IActionResult ThongTinGioHang()
        {
            var shopingCart = HttpContext.Session.GetJson<ShopingCart>("Cart");
            if (shopingCart != null)
            {
                return PartialView(shopingCart.Items);
            }
            return View();
        }

        public IActionResult checkout()
        {
           
            return View();
        }

        [HttpPost]
        public IActionResult checkout(TblOrder tblOrder)
        {
            var shopingCart = HttpContext.Session.GetJson<ShopingCart>("Cart");
            if (shopingCart == null)
            {
                return RedirectToAction("pizza", "home");
            }
            else
            {
                pizzaweb.Models.Tables.pizzawebContext db = new pizzaweb.Models.Tables.pizzawebContext();

                tblOrder.TotalPrice = shopingCart.Items.Sum(x => (x.TotalPrice * x.Quantity));
                tblOrder.CreatedDate = DateTime.Now;

                db.TblOrders.Add(tblOrder);
                db.SaveChanges();

                shopingCart.Items.ForEach(item => db.TblProductOrders.Add(new TblProductOrder
                {
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Name = item.ProductName,
                    OrderId = tblOrder.Id
                }));
                db.SaveChanges();

                return RedirectToAction("succsess", "cart");
            }
        }

        public IActionResult ShowCount()
        {
            ShopingCart cart = HttpContext.Session.GetJson<ShopingCart>("Cart");
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count });
            }
            return Json(new { Count = 0 });
        }

        public IActionResult AddToCart(int id, int quantity)
        {
            var code = new { Succsess = false, msg = "lỗi", code = -1 };
            pizzaweb.Models.Tables.pizzawebContext db = new pizzawebContext();
            var checkProduct = db.TblProducts.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                var shopingCart = HttpContext.Session.GetJson<ShopingCart>("Cart");
                if (shopingCart == null)
                {
                    shopingCart = new ShopingCart();
                }
                var cartItem = shopingCart.Items.FirstOrDefault(m => m.ProductId == id);
                if (cartItem != null)
                {
                    cartItem.Quantity += quantity;
                    cartItem.TotalPrice = cartItem.Price * cartItem.Quantity;
                }
                else
                {
                    ShopingCartItem item = new ShopingCartItem()
                    {
                        ProductId = checkProduct.Id,
                        ProductName = checkProduct.Title,
                        Quantity = quantity

                    };
                    item.ProductImg = checkProduct.Avatar;
                    item.Price = checkProduct.Price;
                    item.TotalPrice = item.Price * item.Quantity;
                    shopingCart.AddToCartShopingCartItem(item, quantity);
                }

                HttpContext.Session.SetJson("Cart", shopingCart);

                code = new { Succsess = true, msg = "Thêm sản phẩm vào giỏ hàng thành công", code = 1 };
            }

            return Json(code);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var code = new { Succsess = false, msg = "", code = -1, Count = 0 };

            var cart = HttpContext.Session.GetJson<ShopingCart>("Cart");
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Succsess = true, msg = "", code = -1, Count = cart.Items.Count };
                }
            }

            return Json(code);
        }

        /*private string GetHmacSha256Hash(string message, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private string ExecPostRequest(string url, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(postData);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }

        }

        [HttpGet]
        public ActionResult CreatePaymentmomo()
        {
            

            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMOBKUN20180529";
            string accessKey = "klm05TvNBzhg7h7j";
            string secretKey = "at67qH6mk8w5Y1nAyMoYKMWACiEi2bsa";
            string orderInfo = "Thanh toán qua MoMo";
            string amount = "1";
            string orderId = DateTime.Now.Ticks.ToString();
            string redirectUrl = "https://webhook.site/b3088a6a-2d17-4f8d-a383-71389a6c600b";
            string ipnUrl = "https://webhook.site/b3088a6a-2d17-4f8d-a383-71389a6c600b";
            string extraData = "";



            partnerCode = partnerCode;
            accessKey = accessKey;
            secretKey = secretKey;
            orderId = orderId;
            orderInfo = orderInfo;
            amount = amount;
            ipnUrl = ipnUrl;
            redirectUrl = redirectUrl;
            extraData = extraData;

            string requestId = DateTime.Now.Ticks.ToString();
            string requestType = "payWithATM";

            // before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey + "&amount=" + amount + "&extraData=" + extraData + "&ipnUrl=" + ipnUrl + "&orderId=" + orderId + "&orderInfo=" + orderInfo + "&partnerCode=" + partnerCode + "&redirectUrl=" + redirectUrl + "&requestId=" + requestId + "&requestType=" + requestType;
            string signature = GetHmacSha256Hash(rawHash, secretKey);

            Dictionary<string, string> data = new Dictionary<string, string>
        {
            { "partnerCode", partnerCode },
            { "partnerName", "Test" },
            { "storeId", "MomoTestStore" },
            { "requestId", requestId },
            { "amount", amount },
            { "orderId", orderId },
            { "orderInfo", orderInfo },
            { "redirectUrl", redirectUrl },
            { "ipnUrl", ipnUrl },
            { "lang", "vi" },
            { "extraData", extraData },
            { "requestType", requestType },
            { "signature", signature }
        };

            string result = ExecPostRequest(endpoint, JsonConvert.SerializeObject(data));
            dynamic jsonResult = JsonConvert.DeserializeObject(result);
            string la = jsonResult.ToString();
            string filePath = @"D:\20paymentUrl.txt";

            // Ghi giá trị của paymentUrl vào tệp
            System.IO.File.WriteAllText(filePath, la);
            // Just an example, please handle the response accordingly
            return Redirect(jsonResult["payUrl"].ToString());

        }*/
    }
}
