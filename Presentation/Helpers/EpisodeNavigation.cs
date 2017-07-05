using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Presentation.Models;

namespace Presentation.Helpers
{
    public class EpisodeNavigation
    {
        static String xmlPath;
        
        static IList<String> episodeList;
        static IList<String> seasonList;

        static Int32 episodeNum;
        static Int32 seasonNum;

        public static void SetNavigation(SeasonEpisodeModel model, String xmlPathNavigation)
        {
            xmlPath = xmlPathNavigation;

            var episode = model.Story.Episode.ID;
            var season = model.Story.Episode.Season.ID;
            
            
            episodeList = getEpisodes(season);
            seasonList = getSeasons();
            

            episodeNum = episodeList.IndexOf(episode);
            var firstEpisode = episodeNum == 0;
            var lastEpisode = episodeNum + 1 == episodeList.Count;


            seasonNum = seasonList.IndexOf(season);
            var firstSeason = seasonNum == 0;
            var lastSeason = seasonNum + 1 == seasonList.Count;


            model.Prev = getOtherEpisodeLink(firstEpisode, firstSeason, true);
            model.Next = getOtherEpisodeLink(lastEpisode, lastSeason, false);


            //return model;
        }


        private static IList<String> getEpisodes(String season)
        {
            var filePath = Path.Combine(xmlPath, "_" + season);

            return Directory
                .GetDirectories(filePath)
                .Select(f =>
                    f.Substring(f.LastIndexOf(@"\") + 1)
                )
                .ToList();
        }

        private static IList<String> getSeasons()
        {
            return Directory
                .GetDirectories(xmlPath, "_*")
                .Select(d =>
                    d.Substring(d.LastIndexOf(@"\") + 2)
                )
                .Where(d => getEpisodes(d).Any())
                .ToList();
        }

        private static SeasonEpisode getOtherEpisodeLink(bool isEdgeEpisode, bool isEdgeSeason, bool previous)
        {
            var seasonEpisode = new SeasonEpisode();

            var diff = previous ? -1 : +1;

            if (isEdgeEpisode)
            {
                if (!isEdgeSeason)
                {
                    seasonEpisode.Season = seasonList[seasonNum + diff];
                    episodeList = getEpisodes(seasonEpisode.Season);
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