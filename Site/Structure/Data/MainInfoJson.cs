using System;
using System.IO;
using Structure.Entities;
using Structure.Entities.Json;
using Structure.Extensions;

namespace Structure.Data
{
    public static class MainInfoJson
    {
		public static void Save(String title, String summary, String folderPath, String seasonID, String episodeID)
        {
            var path = Paths.SceneFilePath(folderPath, seasonID, episodeID, "_");

            new FileInfo(path).CreateIfNotExists("{}");

	        var summaryPart = path.Read<SummaryPart>();

	        summaryPart.Title = title;
	        summaryPart.Summary = summary;
	        summaryPart.Season = seasonID;
			summaryPart.Episode = episodeID;

			path.Write(summaryPart);
        }

        internal static SummaryPart Get(String path, String seasonID, String episodeID)
        {
            return Paths
	            .SceneFilePath(path, seasonID, episodeID, "_")
	            .Read<SummaryPart>();
        }
    }
}
