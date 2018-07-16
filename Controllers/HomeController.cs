using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Contentstack.Core;
using Contentstack.Core.Configuration;
using Microsoft.Extensions.Options;
using MVCDemo.Models;
using MVCDemo;
using Newtonsoft.Json.Linq;

namespace MVCDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ContentstackClient _stack;
        
        public HomeController(ContentstackClient stack, IOptions<ContentstackOptions> contentstackOptions)
       {
              _stack = stack;
        }

        public async Task<IActionResult> Index()
        {
            var query = _stack.ContentType("articles").Query();
            var response = await query.Find();
            dynamic data = JObject.Parse(response.ResultJson);
            ViewBag.article = data;
            return View();
         }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
