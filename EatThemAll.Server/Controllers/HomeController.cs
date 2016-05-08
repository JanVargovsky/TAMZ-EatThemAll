using EatThemAll.Server.Game.Models;
using EatThemAll.Server.Models;
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
            EatThemAllSettingsModel model = new EatThemAllSettingsModel
            {
                Foods = game.Foods,
                Players = game.Players.OrderByDescending(p => p.Score).ToList()
            };

            return View(nameof(Index), model);
        }

        [Route("Home/KillPlayer/{id}")]
        public ActionResult KillPlayer(string id)
        {
            var player = game.Players.FirstOrDefault(p => p.Id == id);

            if (player == null)
                return Index();

            game.Kill(player);

            return Index();
        }

        [Route("Home/AddBot/{count?}")]
        public ActionResult AddNewBot(int? count)
        {
            if (!count.HasValue)
                count = 1;

            for (int i = 0; i < count.Value; i++)
                game.AddNewBot();

            return Index();
        }
    }
}