using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Abstract;
using UrlShortener.Conctere;
using UrlShortener.Models;

namespace UrlShortener.Middleware
{
    public class Reroute
    {
        private readonly RequestDelegate _next;
        public Reroute(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IDatabaseRepository _context)
        {
            
            string curUlr = context.Request.Path.Value.Replace("/", "");


            var match = _context.Urls.GetRecords().Where(x => x.UrlModdedPath.Equals(curUlr));
            if (match.Count() > 0)
            {
                context.Response.Redirect(match.FirstOrDefault().UrlOriginal.AbsoluteUri);
                return;
            }

            await _next.Invoke(context);
        }

    }
}
