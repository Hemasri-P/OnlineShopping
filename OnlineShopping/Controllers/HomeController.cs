using Microsoft.AspNetCore.Mvc;

namespace OnlineShopping.Controllers
{
    public class HomeController : Controller
    {
       //Hello world
       // create a method string
       // That will display the string
       public ViewResult Index()
        {
            ViewData["test1"] = "hi";
            ViewData["test2"] = "sindu";
            ViewBag.Test = "Hemasri";

            //Display all country names in view page with bullets
            //use viewdata and view bag
            String[] CountryNames = { "India", "canda", "US", "UK" };
            ViewData["CountryNames"]=Convert.ToString(CountryNames);
            ViewBag.CountryNames = CountryNames;
            return View();
        }
        public ViewResult Proclink() //can have html tags
        {
            return View();
        }
        public ViewResult ShowDate()
        {
            return View(); 
        }
        //[ActionName("ind")]
        [NonAction]// india method cannot be called from browser
        public string india()
        {
            return " Welcome to India";
        }


        public ViewResult Addnums()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Addnums(string txt1, string txt2)
        {
            int res = int.Parse(txt1 )+int.Parse(txt2);

            ViewData["v"] = res;
            return View();
        }
        public ViewResult printname()
        {
            
            return View();
        }
        [HttpPost]
        public ViewResult printname(string num, string text)
        {
            ViewData ["n"] = num;
            ViewData ["m"] = text;
            return View();
        }


    }
}
