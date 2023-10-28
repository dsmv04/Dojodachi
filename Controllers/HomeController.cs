using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Dojodachi.Models;
using Microsoft.Extensions.Hosting;

namespace Dojodachi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostEnvironment _env;

        public HomeController(IHostEnvironment env)
        {
            _env = env;
        }

        private void setDachiInSession(Dachi dachi = null)
        {
            if (dachi == null)
            {
                dachi = new Dachi();
                HttpContext.Session.SetString("newGame", "true");
            }
            HttpContext.Session.SetInt32("happiness", dachi.Happiness);
            HttpContext.Session.SetInt32("fullness", dachi.Fullness);
            HttpContext.Session.SetInt32("energy", dachi.Energy);
            HttpContext.Session.SetInt32("meals", dachi.Meals);
        }

        private Dachi getDachiFromSession()
        {
            return new Dachi(
                HttpContext.Session.GetInt32("happiness"),
                HttpContext.Session.GetInt32("fullness"),
                HttpContext.Session.GetInt32("energy"),
                HttpContext.Session.GetInt32("meals")
            );
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("newGame") == null)
                setDachiInSession();

            return View(getDachiFromSession());
        }

        [HttpGet("game/{gameType}")]
        public IActionResult Play(string gameType)
        {
            Dachi currDachi = getDachiFromSession();
            TempData["message"] = currDachi.Play(gameType);
            setDachiInSession(currDachi);
            return RedirectToAction("Index");
        }

        [HttpGet("restart")]
        public IActionResult Restart()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
