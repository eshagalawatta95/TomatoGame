﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TomatoGame.Web.Controllers
{
    public class ScoreController : Controller
    {
        // GET: Score
        public ActionResult Index()
        {
            return View();
        }
    }
}