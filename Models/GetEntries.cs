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

namespace MVCDemo.Models
{
    public class GetEntries
    {
        private readonly ContentstackClient _stack;
      //  private int i=1,tempcount=1;
        public GetEntries(ContentstackClient stack)
        {
            _stack = stack;
        }
        
        public async Task<IEnumerable<Entries>> EntriesListBind()
        {
            var query = _stack.ContentType("articles").Query();
            var response = await query.Find();
             
            dynamic data = JObject.Parse(response.ResultJson);
            List<Entries> lst = new List<Entries>();    
            if ( response.Result.Length > 0)
            {   
                for(var i=0;i<response.Result.Length;i++)    
                {    
                    lst.Add(new Entries    
                    {    
                        UID=Convert.ToString(data.entries[i].uid),
                        Title = Convert.ToString( data.entries[i].title),
                        URL = Convert.ToString( data.entries[i].file.url),
                        Date = Convert.ToString( data.entries[i].date),
                        CategoryType = Convert.ToString( data.entries[i].category_type),
                        ArticleDesc = Convert.ToString(data.entries[i].article_description) 
                     });    
                }    
                  
            }    
            return lst;

        }
    }

}