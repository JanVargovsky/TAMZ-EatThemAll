using EatThemAll.Server.Game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EatThemAll.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly Game.Game game = Game.Game.Instance;

        public ActionResult Index()
        {
            int players = 0, bots = 0;
            foreach (var p in game.Players)
            {
                if (p is Bot) bots++;
                else if (p is Player) players++;
            }

            ViewBag.PlayersCount = players;
            ViewBag.BotsCount = bots;
            return View();
        }
    }
}