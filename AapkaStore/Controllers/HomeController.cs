using AapkaStore.Models;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;

namespace AapkaStore.Controllers
{
    public class HomeController : Controller
    {
        public static List<CartItem> TempCart = new List<CartItem>();
        public static User TempUser = null;

        private readonly DbAapkaStoreContext _context;

        public HomeController(DbAapkaStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Categories()
        {
            return View(_context.Categories.ToList());
        }
        public IActionResult Items(int? id)
        {
            var items = _context.Items.Include(x=>x.CatF).ToList();
            if (id != null)
            {
                items = items.Where(x => x.CatFid == id).ToList();
            }
            return View(items);
        }
        public IActionResult ItemDetails(int? id)
        {
            var item = _context.Items.Find(id);
            return View(item);
        }
        public IActionResult ShoppingCart()
        {
            ViewBag.Cart = TempCart;
            return View();
        }
        public IActionResult AddtoCart(int id)
        {
            
                int FoundItem = -1;
                for (int i = 0; i < TempCart.Count; i++)
                {
                    if (TempCart[i].item.ItemId == id)
                    {
                        FoundItem = i;
                    }

                }

                if (FoundItem == -1)
                {
                    TempCart.Add(new CartItem { item = _context.Items.Find(id), quantity = 1 });
                }
                else
                {
                    TempCart[FoundItem].quantity++;
                }
            

            return RedirectToAction("ShoppingCart");
        }
        public IActionResult Remove(int id)
        {
            int FoundItem = -1;
            for (int i = 0; i < TempCart.Count; i++)
            {
                if (TempCart[i].item.ItemId == id)
                {
                    FoundItem = i;
                }
            }
            TempCart.RemoveAt(FoundItem);

            return RedirectToAction("ShoppingCart");
        }
        public IActionResult QtyPlus(int id)
        {
            int FoundItem = -1;
            for (int i = 0; i < TempCart.Count; i++)
            {
                if (TempCart[i].item.ItemId == id)
                {
                    FoundItem = i;
                }
            }


            TempCart[FoundItem].quantity++;

            return RedirectToAction("ShoppingCart");
        }
        public IActionResult QtyMinus(int id)
        {
            int FoundItem = -1;
            for (int i = 0; i < TempCart.Count; i++)
            {
                if (TempCart[i].item.ItemId == id)
                {
                    FoundItem = i;
                }
            }
            if (TempCart[FoundItem].quantity > 1)
            {
                TempCart[FoundItem].quantity--;
            }
            else
            {
                TempData["State"] = "error";
                TempData["Message"] = "Quantity cannot be less than 1";
                return RedirectToAction("ShoppingCart");

            }

            return RedirectToAction("ShoppingCart");
        }
        public IActionResult Checkout(string method)
        {
            if (TempCart.Count == 0)
            {
                TempData["State"] = "warning";
                TempData["Message"] = "Cart is Empty Please add an Item to checkout";
                return Redirect("/Home/ShoppingCart");
            }

            if (TempUser == null)

            {
                TempData["State"] = "warning";
                TempData["Message"] = "You must be logged in to checkout.";

                return Redirect("/Home/Login?return_url=/Home/ShoppingCart");
            }

            
            //ORDER SAVING ================================================================

            Order order = new Order()
            {
                UserFid = TempUser.UserId,
                Date = DateTime.Now.Date,
                Time = DateTime.Now.TimeOfDay,
                Status = "Pending"
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            double total = 0;
            for (int i = 0; i < TempCart.Count; i++)
            {
                total = total + (TempCart[i].quantity * TempCart[i].item.SalePrice);
                OrderDetail detail = new OrderDetail()
                {
                    OrderFid = _context.Orders.Max(x => x.OrderId),
                    ItemFid = TempCart[i].item.ItemId,
                    Quantity = TempCart[i].quantity
                };
                _context.OrderDetails.Add(detail);
                _context.SaveChanges();


                // Reducing the Quantity=================================================

                _context.Items.Find(TempCart[i].item.ItemId).Quantity = _context.Items.Find(TempCart[i].item.ItemId).Quantity - TempCart[i].quantity;
                _context.SaveChanges();
            }

            string ConfirmedOrderID = _context.Orders.Max(x => x.OrderId).ToString();


            //EMPTY CART======================================================================
            TempCart.Clear();


            if (method == "COD")
            {
                return Redirect("/Home/OrderConfirmed/" + ConfirmedOrderID);

            }
            else
            {
                return Redirect("https://www.san_contextox.paypal.com/cgi-bin/webscr?cmd=_xclick&amount=" + total / 226.98 + "&business=JanjuaTailors@Shop.com&item_name=ToyLand&return=https://localhost:44304/Home/OrderConfirmed/" + ConfirmedOrderID);

            }


        }
        public IActionResult OrderConfirmed(int? id)
        {
            if (id == null)
            {
                id = _context.Orders.Max(x => x.OrderId);
            }

            TempData["State"] = "success";
            TempData["Message"] = "Order has confirmed. It will be delivered soon order details are as under";



            var order = _context.Orders.Where(x=>x.OrderId == id).Include(x=>x.UserF).FirstOrDefault();
            ViewBag.Cart = _context.OrderDetails.Where(x => x.OrderFid == order.OrderId).Include(x=>x.ItemF).ToList();

            return View(order);
        }
        public IActionResult Login(string return_url)
        {
            ViewBag.return_url = return_url;
            return View();
        }
        public IActionResult LoginVerify(string email, string password, string type, string return_url)
        {
            if (return_url == null || return_url == "")
            {
                return_url = "/Home/";
            }
            var check = _context.Users.FirstOrDefault(x => x.Email == email && x.Password == password && x.Type == type);
            if (check == null)
            {
                return Content("<script>alert('Email or Password Incorrect....');window.history.back();</script>");
            }
            else
            {
                if (type == "Customer")
                {
                    TempUser = check;
                    return Redirect(return_url);
                }
               
                if (type == "Admin")
                {
                    return Redirect("/Adm_Reports/PLSReport");
                }
                return Redirect("/Home/");
            }
        }
        public IActionResult Register()
        {

            return View();
        }
        public IActionResult CreateData(string name, string phone, string address, string email, string password, string cpassword, string type, int? ddlShops)
        {
            if (password != cpassword)
            {
                return Content("<script>alert('Passwords do not match...');window.history.back();</script>");
            }



            var check = _context.Users.FirstOrDefault(x => x.Email == email && x.Type == type);
            if (check != null)
            {
                return Content("<script>alert('This email with same role is already registered....');window.history.back();</script>");

            }

            var c = new User()
            {
                Name = name,
                Phone = phone,
                Address = address,
                Email = email,
                Password = password,
                Type = type,
                Status = "Active",
                Details = "N/A",
                Image = "N/A"

            };

            _context.Users.Add(c);
            _context.SaveChanges();

            return Redirect("/Home/Login");
        }
        public IActionResult Logout()
        {
            TempUser = null;

            return Redirect("/Home");
        }
        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}