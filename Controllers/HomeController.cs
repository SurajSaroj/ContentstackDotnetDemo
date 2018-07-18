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
using Contentstack.Core.Models;


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
            ViewBag.articlelength = response.Result.Length;
            return View();
        }

        public async Task<IActionResult> News(string id)
        {
            Entry entry = _stack.ContentType("articles").Entry(id);
            await entry.Fetch().ContinueWith((entryResult) =>
            {
                dynamic data = JObject.Parse(entryResult.Result.ToJson());
                ViewBag.ArticleData = data;
            });

            return View();
        }

       


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
