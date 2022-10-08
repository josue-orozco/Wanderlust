using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Wanderlust.Models
{
    public class MockVideoRepository : IVideoRepository
    {
        private string baseUrl = "https://www.youtube.com/embed/";

        // Append to end of url so autoplay is enabled.
        // Youtube has rules/conditions where videos cannot be autoplayed with sound unless the webpage is interacted with first
        // Workaround is to autoplay video muted.
        // Future Options:
        // 1)   A video can start with sound if the user has interacted with the domain (click, tap, etc.) so have a loading page
        //      introducing webpage and then have to click on something to enter which in turn will start videos with sound
        // 2)   Start webpage as mute and then inform user to unmute with a popup
        private string setAutoplay = "?&autoplay=1";
        private string setAutoplayMuted = "?&autoplay=1&mute=1";

        public async Task<IEnumerable<string>> GetVideo()
        {
            IEnumerable<string> videos;
            
            try
            {
                Task<IEnumerable<string>> getVideoTask = Run();
                videos = await getVideoTask;
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Debug.WriteLine("Error: " + e.Message);
                }

                // Add Rick Roll Video
                List<string> rickRoll = new List<string> { "https://www.youtube.com/embed/dQw4w9WgXcQ?&autoplay=1" };
                videos = rickRoll;
            }

            return videos;
        }

        private async Task<IEnumerable<string>> Run()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {

                ApplicationName = this.GetType().ToString()
            });

            // Add filters to search here
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = "Toyota Tacoma";
            searchListRequest.MaxResults = 5;
            searchListRequest.Type = "video";
            searchListRequest.VideoEmbeddable = SearchResource.ListRequest.VideoEmbeddableEnum.True__;
            searchListRequest.SafeSearch = SearchResource.ListRequest.SafeSearchEnum.Strict;

            // Add to set video location request
            //searchListRequest.Location = "";
            //searchListRequest.LocationRadius = "";

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();

            // Adding results to a list as url
            foreach (var searchResult in searchListResponse.Items)
            {
                videos.Add(String.Format("{0}{1}{2}", this.baseUrl, searchResult.Id.VideoId, this.setAutoplay));
            }

            Debug.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));

            return videos;
        }
    }
}
