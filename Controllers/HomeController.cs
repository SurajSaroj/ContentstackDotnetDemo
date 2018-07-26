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
using X.PagedList;

namespace MVCDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ContentstackClient _stack;
        private bool doOnce= false;
        public HomeController(ContentstackClient stack) => _stack = stack;
        public async Task<IActionResult> Index(int? id)
        {
            var pageNumber = id ?? 1;
            GetEntries GetEnt = new GetEntries(_stack);      
            var objEntriesList =await GetEnt.EntriesListBind();   
            var Articles = objEntriesList.ToPagedList(pageNumber, 5); 
            ViewBag.article = Articles;
            if(!doOnce){
                try{
                    await GetTitle();
                    await GetFooter();
                 }catch(Exception ex){  }
              doOnce=true;
            } 
            return View();
        }
 
        public async Task<PartialViewResult> GetTitle()
        {
            string title=null;
            Entry entry = _stack.ContentType("header").Entry("bltf7a51839a49b9b3c");
            await entry.Fetch().ContinueWith((entryResult) =>
            {ViewBag.title = entryResult.Result.Title;
                 title =  entryResult.Result.Title;
            });
            return PartialView("~/Views/Shared/_HeaderNav.cshtml",new Footer{ AppTitle = title });
        }

        public async Task<PartialViewResult> GetFooter()
        {
            dynamic footer=null;
            Entry entry = _stack.ContentType("footer").Entry("blt00fe6b4410d947d3");
            await entry.Fetch().ContinueWith((entryResult) =>
            {
                 footer = JObject.Parse(entryResult.Result.ToJson());

            });
            return PartialView("~/Views/Shared/_FooterNav.cshtml",new Footer{ FooterData = footer });
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
