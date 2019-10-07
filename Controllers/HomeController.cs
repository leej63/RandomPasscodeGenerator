using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RandomPasscodeGenerator.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RandomPasscodeGenerator.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Password = GeneratePasscode(14);
            if(HttpContext.Session.GetInt32("count") == null)
            {
                HttpContext.Session.SetInt32("count", 0);
            }
            int? c = HttpContext.Session.GetInt32("count");
            c++;
            HttpContext.Session.SetInt32("count", (int)c);
            ViewBag.Count = HttpContext.Session.GetInt32("count");
            return View("index");
        }

        public string GeneratePasscode(int length)
        {
            Random rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] charArray = new char[length];
            for(int i = 0; i < length; i++)
            {
                charArray[i] = chars[rand.Next(chars.Length)];
            }
            return string.Join("", charArray);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
