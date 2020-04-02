using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdvertisementHW.Models;
using AdvertisementModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AdvertisementHW.Controllers
{
    public class HomeController : Controller
    {
        string _connectionString = (@"Data Source=.\sqlexpress;Initial Catalog=Advertise;Integrated Security=True;");
        public IActionResult Index()
        {

            ListingManager lm = new ListingManager(_connectionString);
            List<Item> items = lm.GetAllItems().OrderByDescending(d => d.Date).ToList();
            
            if (HttpContext.Session.Get("List") != null)
            {
                string session = HttpContext.Session.Get("List").ToString();
                List<string> SessionList = session.Split(',').ToList();
                ListItemAndString l = new ListItemAndString
                {
                    items = items,
                    Session = SessionList
                };
                return View(l);
            }
            else
            {
                ListItemAndString l = new ListItemAndString
                {
                    items = items,
                    Session = null
                };
                return View(l);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AddItem()
        {
            return View();
        }
        public IActionResult SubmitItem(Item I)
        {
            ListingManager lm = new ListingManager(_connectionString);
            int Id = lm.AddItem(I);
            string Idstring = Id.ToString();
            Idstring += ",";
            if (HttpContext.Session.Get("List") == null)
            {
                HttpContext.Session.Set("List", Idstring);
            }
            else
            {
                string ItemIds = HttpContext.Session.Get("List").ToString();
                List<string> ids = ItemIds.Split(',').ToList();
                ids.Add(Idstring);
                HttpContext.Session.Set("List", ids);
            }
            return Redirect("/Home/Index");
        }
       
    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
