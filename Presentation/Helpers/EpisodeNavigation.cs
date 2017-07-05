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

        static IList<String> seasonList;
        static IList<String> episodeList;
        static IList<String> sceneList;

        static Int32 seasonNum;
        static Int32 episodeNum;
        static Int32 sceneNum;

        public static void SetNavigation(SeasonEpisodeModel model, String xmlPathNavigation)
        {
            xmlPath = xmlPathNavigation;

            var season = model.Story.Episode.Season.ID;
            var episode = model.Story.Episode.ID;
            var scene = model.Story.ID;


            seasonList = getSeasons();
            episodeList = getEpisodes(season);
            sceneList = getScenes(season, episode);


            seasonNum = seasonList.IndexOf(season);
            episodeNum = episodeList.IndexOf(episode);
            sceneNum = sceneList.IndexOf(scene);
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

        private static IList<String> getScenes(String season, String episode)
        {
            var filePath = Path.Combine(xmlPath, "_" + season, episode);

            return Directory
                .GetFiles(filePath, "*.xml")
                .Select(d =>
                    d.Substring(d.LastIndexOf(@"\") + 1, 1)
                )
                .Where(d => d != "_")
                .ToList();
        }
    }
}