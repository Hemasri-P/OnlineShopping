using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class OnlineshopController : Controller
    {//IAction result returns many things=>string , view , json, JavaScript etc...
        OnlishopdbContext dc = new OnlishopdbContext();
        public List<Product> li = new List<Product>();
        public List<Userorder> order = new List<Userorder>();
        OnlishopdbContext fb= new OnlishopdbContext();
        public IActionResult Home()
        {
           
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string txt1, string txt2)
        {
            var res=(from t in dc.Registers
                    where t.Uname == txt1 && t.Password==txt2
                    select t).Count();
            if (res > 0)
            {
                HttpContext.Session.SetString("uid", txt1);
                return RedirectToAction("ViewProduct");
            }
            else
            {
                ViewData["msg"] = "InValid Credentials , Pls try Again!!";
            }

            return View();
        }
        public IActionResult LogOut()
        {
            ViewData["msg"] = "Thank you !!";
            HttpContext.Session.Remove("uid");

            return View();
        }
        
       


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Register r )
        {
            if (ModelState.IsValid)
            {
                dc.Registers.Add(r);
            }
                int i = dc.SaveChanges();
                if (i > 0)
                {
                    ViewData["msg"] = "New User created successfully!!";
                }
                else
                {
                    ViewData["msg"] = "Could not create user , try Again!!";
                }
            
                       return View();
        }
        public IActionResult ViewProduct()
        {
            var res = (from t in dc.Products
                      select t).ToList();
            
            return View(res);
        }
        public IActionResult Buy(string pid)
        {
            if (HttpContext.Session.GetString("uid") == null)
            {
                return RedirectToAction("Login");

            }
            HttpContext.Session.SetString("proid", "pid");
            var res= (from t in dc.Products
                     where t.Pid==pid
                     select t).ToList();

            return View(res);
        }
        [HttpPost]
        public IActionResult Buy(int qty)
        {
            string pid = HttpContext.Session.GetString("proid");
            
            Userorder obj= new Userorder();
            Feedbacktbl fb= new Feedbacktbl();

            obj.Pid = Request.Query["pid"];
            obj.Username = HttpContext.Session.GetString("uid");

            obj.Transdate = DateOnly.FromDateTime(DateTime.Now);
            obj.Qty = qty;

            fb.Username = HttpContext.Session.GetString("uid");
            fb.Pid =Request.Query["pid"];
            fb.Fstatus =false;

            dc.Userorders.Add(obj);
            dc.Feedbacktbls.Add(fb);
            HttpContext.Session.SetString("fid",Convert.ToString(fb.Id));
            int i = dc.SaveChanges();
            if (i > 0)
            {
                ViewData["msg"] = "Order placed Successfully";
            }
            else
            {
                ViewData["msg"] = "Order failed, try Again";
            }


            return View();
           
        }

        public IActionResult Feedback()
        {
            
            
           
            if (HttpContext.Session.GetString("uid") == null)
            {
                return RedirectToAction("Login");
            }
            var items = new List<SelectListItem>();


            var res = (from t in fb.Feedbacktbls
                       where t.Fstatus== false && t.Username == HttpContext.Session.GetString("uid")
                       select t).ToList();

            foreach (var item in res)
            {
                items.Add(new SelectListItem{ Value = item.Pid });
            }

            ViewBag.DropDownData = items;

            return View();
        }
        public IActionResult Search(string txtsearch)
        {
            var res=(from t in dc.Products
                    where t.Pname.Contains(txtsearch)
                    select t).ToList();

            var r=( from t in dc.Products
                   where t.Pname.Contains(txtsearch)
                   select t).Count();
            if (r > 0)
            {
                ViewData["msg"] = " Number of products found " +  r;
            }
            else
            {
                ViewData["msg"] = "No Products Found!!";
            }
            return View(res);
        }
       
        public IActionResult Error()
        {
            return View();
        }
        public PartialViewResult Testpartial()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult Testpartial(Feedbacktbl f)
        {
            var res = (from t in dc.Feedbacktbls
                       where t.Pid == f.Pid && t.Fstatus==false && t.Username == HttpContext.Session.GetString("uid")
                       select t).First();
            res.Fstatus = true;
            res.Usermessage = f.Usermessage;
            res.Ratings= f.Ratings;

            dc.Feedbacktbls.Update(res);

                int i = dc.SaveChanges();
                if (i > 0)
                {
                    ViewData["msg"] = "Thank you for your valuable feedback";
                }
                else
                {
                    ViewData["msg"] = "Action, deducted pls try again!!";
                }
                return View();
            }
       public IActionResult testing()
        {
            throw new DivideByZeroException();
        }


    }
}
