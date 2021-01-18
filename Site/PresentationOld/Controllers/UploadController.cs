using System;
using System.Web.Mvc;
using Presentation.Models;
using Structure.Helpers;

namespace Presentation.Controllers
{
	public class UploadController : Controller
	{
		public ActionResult Upload(String seasonID, String episodeID, String blockID)
		{
			var model = new UploadModel(seasonID, episodeID, blockID);

			return View(model);
		}

		[HttpPost]
		public ActionResult Upload(UploadModel model)
		{
			if (ModelState.IsValid)
			{
				model.UploadBlock();

				if (String.IsNullOrEmpty(model.Result))
				{
					var route = RouteStories.With(
						model.SeasonID, model.EpisodeID, model.BlockID
					);

					var path = Url.Action("Episode", "Season", route);

					return Redirect(Config.Site + path);
				}
			}

			return View(model);
		}

	}
}
