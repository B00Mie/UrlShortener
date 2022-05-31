using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Abstract;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        private IDatabaseRepository repo;
        public UrlsController(IDatabaseRepository context)
        {
            repo = context;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult GetAll()
        {
            UserModel user = (UserModel)HttpContext.Items["User"];
            if (user != null)
            {
                var data = repo.Urls.GetRecords().Where(x => x.UserId == user.Id);

                return Ok(data);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Create")]
        public ActionResult Create(UrlModel url)
        {
            Uri urlOriginal = new Uri(url.UrlOriginal.AbsoluteUri);
            string moddedUrl = Helpers.ShortenUrl.Shorten();

            while (repo.Urls.GetRecords().Where(x => x.UrlModdedPath == moddedUrl).Count() > 0)
            {
                moddedUrl = Helpers.ShortenUrl.Shorten();
            }

            UserModel user = (UserModel)HttpContext.Items["User"];

            url.UrlModdedPath = moddedUrl;
            url.UrlModded = $"{Request.Host.Value}/{moddedUrl}";

            url.UrlOriginalHost = urlOriginal.Host;
            url.UrlOriginalPath = urlOriginal.AbsolutePath;
            url.UserId = user.Id;
            try
            {
                repo.Urls.CreateRecord(url);
                repo.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("Edit")]
        public ActionResult Edit(UrlModel url)
        {
            try
            {
                UrlModel urlOriginal = repo.Urls.GetRecord(url.Id);
                UserModel user = (UserModel)HttpContext.Items["User"];

                if (url.UrlOriginal != urlOriginal.UrlOriginal && user.Id == urlOriginal.UserId)
                {
                    url.UrlModded = Helpers.ShortenUrl.Shorten();
                    repo.Urls.UpdateRecord(url);
                    repo.Save();

                    return Ok();
                }


                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                UserModel user = (UserModel)HttpContext.Items["User"];
                UrlModel record = repo.Urls.GetRecord(id);

                if (user.Id ==record.UserId)
                {
                    repo.Urls.DeleteRecord(id);
                    repo.Save();
                    return Ok();
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }


}
