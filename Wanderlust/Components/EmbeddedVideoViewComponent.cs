using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wanderlust.Models;
using Wanderlust.ViewModels;

namespace Wanderlust.Components
{
    public class EmbeddedVideoViewComponent : ViewComponent
    {
        private readonly IVideoRepository _videoRepository;
        public EmbeddedVideoViewComponent(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public IViewComponentResult Invoke()
        {
            Random rnd = new Random();
            Task<IEnumerable<string>> videos = _videoRepository.GetVideo();

            string video = videos.Result.ElementAt(rnd.Next(0, videos.Result.Count()));
            HomeViewModel homeViewModel = new HomeViewModel
            {
                //VideoUrl = videos.Result.First()
                VideoUrl = videos.Result.ElementAt(rnd.Next(0, videos.Result.Count()))
            };

            return View(homeViewModel);
        }

    }
}
