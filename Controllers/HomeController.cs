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
        //  private int i=1,tempcount=1;
        public HomeController(ContentstackClient stack, IOptions<ContentstackOptions> contentstackOptions) => _stack = stack;

        /*    [HttpGet]    
            public async Task<IActionResult> Index(int? id)    
            {    
                int pageSize = 5;
                int pageIndex = 1;
                pageIndex = id.HasValue ? Convert.ToInt32(id) : 1 ;
                IPagedList<Entries> ent = null;
                GetEntries buisnessLogic = new GetEntries(_stack);      
                Entries objEntries = new Entries();      
                List<Entries> objEntriesList = new List<Entries>();      
                objEntriesList =await buisnessLogic.EntriesListBind();      
                objEntries.ent = objEntriesList;
                ent=objEntriesList.ToPagedList(pageIndex,pageSize);
                return View(ent);     
             }   */


        public async Task<IActionResult> Index(int? id)
        {
             var pageNumber = id ?? 1;
            GetEntries buisnessLogic = new GetEntries(_stack);      
            var objEntriesList =await buisnessLogic.EntriesListBind();   

         //   var query = _stack.ContentType("articles").Query();
         //   var response = await query.Find();
           // var products = MyProductDataSource.FindAllProducts();
         //   dynamic data = JObject.Parse(response.ResultJson);
          /*  if(id==1)
            {ViewBag.startindex=1;
             ViewBag.endindex=5;}else if(id==2){
             ViewBag.startindex=6;
             ViewBag.endindex=10;}else if(id==3){
             ViewBag.startindex=11;
             ViewBag.endindex=15;}*/

            var Articles = objEntriesList.ToPagedList(pageNumber, 5); 

            ViewBag.article = Articles;

            //ViewBag.article = data;
         //   ViewBag.articlelength = response.Result.Length;
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
