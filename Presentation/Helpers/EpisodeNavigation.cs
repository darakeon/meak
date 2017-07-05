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
            var firstSeason = seasonNum == 0;
            var lastSeason = seasonNum + 1 == seasonList.Count;
            

            episodeNum = episodeList.IndexOf(episode);
            var firstEpisode = episodeNum == 0;
            var lastEpisode = episodeNum + 1 == episodeList.Count;


            sceneNum = sceneList.IndexOf(scene);
            var firstScene = sceneNum == 0;
            var lastScene = sceneNum + 1 == sceneList.Count;


            model.Prev = getOtherEpisodeLink(firstSeason, firstEpisode, firstScene, true);
            model.Next = getOtherEpisodeLink(lastSeason, lastEpisode, lastScene, false);


            //return model;
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



        private static SeasonEpisodeScene getOtherEpisodeLink(Boolean isEdgeSeason, Boolean isEdgeEpisode, Boolean isEdgeScene, Boolean previous)
        {
            var ses = new SeasonEpisodeScene();

            var diff = previous ? -1 : +1;

            if (!isEdgeScene)
            {
                ses.Season = seasonList[seasonNum];
                ses.Episode = episodeList[episodeNum];
                ses.Scene = sceneList[sceneNum + diff];

                return ses;
            }
            
            if (!isEdgeEpisode)
            {
                ses.Season = seasonList[seasonNum];
                ses.Episode = episodeList[episodeNum + diff];

                sceneList = getScenes(ses.Season, ses.Episode);
                ses.Scene = previous ? sceneList.Last() : sceneList.First();

                return ses;
            }
            
            if (!isEdgeSeason)
            {
                ses.Season = seasonList[seasonNum + diff];

                episodeList = getEpisodes(ses.Season);
                ses.Episode = previous ? episodeList.Last() : episodeList.First();

                sceneList = getScenes(ses.Season, ses.Episode);
                ses.Scene = previous ? sceneList.Last() : sceneList.First();

                return ses;
            }

            return null;
        }
    }
}