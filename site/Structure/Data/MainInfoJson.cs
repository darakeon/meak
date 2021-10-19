using System;
using System.IO;
using Structure.Entities.Json;
using Structure.Extensions;

namespace Structure.Data
{
	public static class MainInfoJson
	{
		public static void Save(
			String title,
			String synopsis,
			String summary,
			Int32? breaks,
			String folderPath,
			String seasonID,
			String episodeID
		)
		{
			var path = Paths.BlockFilePath(folderPath, seasonID, episodeID, "_");

			new FileInfo(path).CreateIfNotExists("{}");

			var summaryPart = path.Read<SummaryPart>();

			summaryPart.Title = title;
			summaryPart.Synopsis = synopsis;
			summaryPart.Summary = summary;
			summaryPart.Breaks = breaks;
			summaryPart.Season = seasonID;
			summaryPart.Episode = episodeID;

			path.Write(summaryPart);
		}

		internal static SummaryPart Get(String path, String seasonID, String episodeID)
		{
			return Paths
				.BlockFilePath(path, seasonID, episodeID, "_")
				.Read<SummaryPart>();
		}
	}
}
