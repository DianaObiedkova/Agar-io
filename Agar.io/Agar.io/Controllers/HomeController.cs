using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Agar.io.Models;

namespace Agar.io.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Game Game = Game.Instance;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //public IActionResult KillPlayer(string id)
        //{
        //    var player = Game.players.FirstOrDefault(x => x.Id == id);
        //    if (player is null) return Index();
        //    Game.EatOrRemovePlayer(player);
        //    return Index();
        //}

        //public IActionResult AddNewBot(int? count)
        //{
        //    if (!count.HasValue)
        //        count = 1;

        //    for (int i = 0; i < count.Value; i++)
        //        Game.AddNewBotPlayer();

        //    return Index();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
