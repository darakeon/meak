using System;
using System.Web.Mvc;
using Presentation.Models;
using Structure.Helpers;

namespace Presentation.Controllers
{
    public class UploadController : Controller
    {
        public ActionResult Upload(String seasonID, String episodeID, String sceneID)
        {
            var model = new UploadModel(seasonID, episodeID, sceneID);

            return View(model);
        }

        [HttpPost]
        public ActionResult Upload(UploadModel model)
        {
            if (ModelState.IsValid)
            {
                model.UploadScene();

                if (String.IsNullOrEmpty(model.Result))
                {
                    var path = Url.RouteUrl("Scene", new { controller = "Season", action = "Episode", seasonID = model.SeasonID, episodeID = model.EpisodeID, sceneID = model.SceneID});

                    return Redirect(Config.Site + path);
                }
            }

            return View(model);
        }

    }
}
