using EatThemAll.Server.Game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatThemAll.Server.Models
{
    public class EatThemAllSettingsModel
    {
        public List<Player> Players { get; set; }
        public List<Food> Foods { get; set; }
    }
}