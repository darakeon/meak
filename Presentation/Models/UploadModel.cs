using System;
using Presentation.Controllers;
using Structure.Data;

namespace Presentation.Models
{
    public class UploadModel : BaseModel
    {
        public String SeasonID { get; set; }
        public String EpisodeID { get; set; }
        public String SceneID { get; set; }

        public UploadModel() { }

        public UploadModel(String seasonID, String episodeID, String sceneID)
        {
            SeasonID = seasonID;
            EpisodeID = episodeID;
            SceneID = sceneID;
        }

        public String Result { get; set; }
        public String Password { get; set; }

        public void UploadScene()
        {
            var ftp = new FtpHelper(SeasonID, EpisodeID, SceneID, Password);

            Result = ftp.Upload();
        }

    }
}