using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Abstract;
using UrlShortener.Conctere;
using UrlShortener.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IJWTUserService _userService;
        private IDatabaseRepository repo;
        public UsersController(IJWTUserService userService, IDatabaseRepository context)
        {
            _userService = userService;
            repo = context;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Authenticate")]
        public IActionResult Authenticate([FromBody] UserModel user)
        {
            var token = _userService.Authenticate(user);

            if (token == null)
            {
                return RedirectToAction("Authenticate");
            }
            //Request.Headers["Authorization"] = $"Bearer {token}";
            Response.Cookies.Append("Authorization", $"Bearer {token}");

            return Ok(token);
        }

        [HttpGet]
        [Route("LogOut")]
        public IActionResult LogOut()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] UserModel user)
        {
            if (repo.Users.GetRecords().Where(x => x.Login == user.Login).FirstOrDefault() != null)
                return RedirectToAction("Register"); //need to add model errors
            repo.Users.CreateRecord(user);
            repo.Save();

            return Ok();
        }
    }
}
