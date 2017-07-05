using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Presentation.Models;

namespace Presentation.Helpers
{
    public class EpisodeNavigation
    {
        static String XMLPath;
        
        static IList<String> episodeList;
        static IList<String> seasonList;

        static Int32 episodeNum;
        static Int32 seasonNum;

        public static void SetNavigation(SeasonEpisodeModel model, String xmlPath)
        {
            XMLPath = xmlPath;

            var episode = model.Story.ID;
            var season = model.Story.Season.ID;
            
            
            episodeList = GetEpisodes(season);
            seasonList = GetSeasons();
            

            episodeNum = episodeList.IndexOf(episode);
            var firstEpisode = episodeNum == 0;
            var lastEpisode = episodeNum + 1 == episodeList.Count;


            seasonNum = seasonList.IndexOf(season);
            var firstSeason = seasonNum == 0;
            var lastSeason = seasonNum + 1 == seasonList.Count; ;


            model.Prev = GetOtherEpisodeLink(firstEpisode, firstSeason, true);
            model.Next = GetOtherEpisodeLink(lastEpisode, lastSeason, false);


            //return model;
        }


        private static IList<String> GetEpisodes(String season)
        {
            var filePath = Path.Combine(XMLPath, "_" + season);

            return Directory
                .GetFiles(filePath, "*.xml")
                .Select(f =>
                    f.Substring(f.LastIndexOf(@"\") + 1, 2)
                )
                .ToList();
        }

        private static IList<String> GetSeasons()
        {
            return Directory
                .GetDirectories(XMLPath, "_*")
                .Select(d =>
                    d.Substring(d.LastIndexOf(@"\") + 2)
                )
                .Where(d => GetEpisodes(d).Any())
                .ToList();
        }

        private static SeasonEpisode GetOtherEpisodeLink(bool isEdgeEpisode, bool isEdgeSeason, bool previous)
        {
            SeasonEpisode seasonEpisode = new SeasonEpisode();

            var diff = previous ? -1 : +1;

            if (isEdgeEpisode)
            {
                if (!isEdgeSeason)
                {
                    seasonEpisode.Season = seasonList[seasonNum + diff];
                    episodeList = GetEpisodes(seasonEpisode.Season);
                    seasonEpisode.Episode = previous ? episodeList.Last() : episodeList.First();
                }
                else
                {
                    seasonEpisode = null;
                }
            }
            else
            {
                seasonEpisode.Season = seasonList[seasonNum];
                seasonEpisode.Episode = episodeList[episodeNum + diff];
            }

            return seasonEpisode;
        }
    }
}