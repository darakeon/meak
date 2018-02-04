using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Structure.Entities.System;

namespace Presentation.Models.General
{
    public class MenuModel
    {
        public MenuModel(String path)
        {
            var seasons = Directory
                    .GetDirectories(path, "_*")
                    .ToList();

            SeasonList = new List<Season>();

            seasons.ForEach(
                d => SeasonList.Add(new Season(d))
            );

            SeasonList = SeasonList
                .Where(s => s.EpisodeList.Any())
                .ToList();
        }

        public String Route { get; set; }
        public IList<Season> SeasonList { get; set; }
    }
}