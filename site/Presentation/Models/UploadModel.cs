using System;
using Structure.Data;

namespace Presentation.Models
{
	public class UploadModel : BaseModel
	{
		public String SeasonID { get; set; }
		public String EpisodeID { get; set; }
		public String BlockID { get; set; }

		public UploadModel() { }

		public UploadModel(String seasonID, String episodeID, String blockID)
		{
			SeasonID = seasonID;
			EpisodeID = episodeID;
			BlockID = blockID;
		}

		public String Result { get; set; }
		public String Password { get; set; }

		public void UploadBlock()
		{
			var ftp = new FtpHelper(SeasonID, EpisodeID, BlockID, Password);

			Result = ftp.Upload();
		}

	}
}