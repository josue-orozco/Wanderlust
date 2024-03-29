﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Wanderlust.Models;
using Wanderlust.ViewModels;

namespace Wanderlust.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVideoRepository _videoRepository;
        
        public HomeController(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public IActionResult Index()
        {
            Random rnd = new Random();
            Task<IEnumerable<string>> videos = _videoRepository.GetVideo();

            string video = videos.Result.ElementAt(rnd.Next(0, videos.Result.Count()));
            HomeViewModel homeViewModel = new HomeViewModel
            {
                //VideoUrl = videos.Result.First()
                VideoUrl = videos.Result.ElementAt(rnd.Next(0, videos.Result.Count()))
            };

            // cant pass it here since this page is loaded only once. will have to do an ajax call when user inputs length or maybe we select at certain increments
            // adding video length
            ViewBag.VideoLength = 20000;

            return View(homeViewModel);
        }

        public IActionResult ReloadEvents()
        {
            Random rnd = new Random();
            //ViewBag.VideoLength = rnd.Next(1, 6) * 10000;
            ViewBag.VideoLength = 10000;
            return ViewComponent("EmbeddedVideo");
        }
    }
}
