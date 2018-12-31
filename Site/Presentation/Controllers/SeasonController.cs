using System;
using System.Linq;
using System.Web.Mvc;
using Presentation.Models;
using Presentation.Helpers;
using Structure.Data;
using Structure.Helpers;

namespace Presentation.Controllers
{
	[StoriesAuthorize]
	public class SeasonController : Controller
	{
		public ActionResult Index(String seasonID, String returnUrl)
		{
			return String.IsNullOrEmpty(seasonID)
				? index(returnUrl)
				: episodes(seasonID);
		}

		private ActionResult index(String returnUrl)
		{
			var model = new SeasonIndexModel
			{
				LogOn =
				{
					Is = !String.IsNullOrEmpty(returnUrl),
					ReturnUrl = returnUrl
				},
			};

			return View("Intro", model);
		}

		private ActionResult episodes(String seasonID)
		{
			var model = new SeasonSeasonModel(seasonID);

			if (!model.EpisodeList.Any())
			{
				return RedirectToAction("Episode",
					new {
						seasonID, 
						episodeID = "01", 
					});
			}

			return View("Summary", model);
		}

		public ActionResult Episode(String seasonID, String episodeID, String sceneID)
		{
			SeasonEditEpisodeModel model;

			try
			{
				model = new SeasonEditEpisodeModel(seasonID, episodeID, sceneID);
			}
			catch (Exception e)
			{
				if (UrlUserType.IsAuthor()) 
					throw;

				return View("Error", new ErrorModel { Message = e.Message });
			}

			model.GetSuggestionLists();

			if (UrlUserType.IsAuthor())
			{
				model.TabIndex = 1;
				return View("Author/Episode", model);
			}

			return View(model);
		}



		[HttpPost]
		public void EditTitle(SeasonEpisodeModel model, String seasonID, String episodeID)
		{
			MainInfoJson.Save(model.Story.Title, model.Story.Summary, model.Paths.Json, seasonID, episodeID);
		}



		[HttpPost]
		public void EditScene(SeasonEpisodeModel model, String seasonID, String episodeID, String sceneID)
		{
			var xml = new SceneJson(model.Paths.Json, seasonID, episodeID, sceneID) { Scene = model.Story[sceneID] };

			xml.WriteStory();
		}



		[HttpPost]
		public ActionResult Adder(Int32 scene, String type, String subtype, Int32? paragraph, Int32? teller, Int32? talk, Int32? piece)
		{
			var adder = new Adder();

			adder.SetScene(scene);

			switch ((type + subtype).ToLower())
			{
				case "pieceteller":
					adder.SetPieceTeller(scene, piece ?? 0, teller ?? 0, talk ?? 0);
					break;
				case "piecetalk":
					adder.SetPieceTalk(scene, piece ?? 0, talk ?? 0, teller ?? 0);
					break;
				case "paragraphteller":
					adder.SetParagraphTeller(scene, paragraph ?? 0, teller ?? 0, talk ?? 0);
					break;
				case "paragraphtalk":
					adder.SetParagraphTalk(scene, paragraph ?? 0, talk ?? 0, teller ?? 0);
					break;
				default:
					throw new Exception("Unknown Adder Type.");
			}

			return PartialView(adder.View, adder.Model);
		}



		public ActionResult AddEpisode()
		{
			var model = new SeasonAddEpisodeModel();

			return View("Author/AddEpisode", model);
		}

		[HttpPost]
		public ActionResult AddEpisode(SeasonAddEpisodeModel model)
		{
			if (!model.SeasonEpisode.IsValid())
			{
				return RedirectToAction("AddEpisode");
			}


			var xml = new SceneJson(model.Paths.Json, model.SeasonEpisode.Season, model.SeasonEpisode.Episode);

			xml.AddNewStory(model.Title);


			return RedirectToAction("Episode", new { season = xml.Scene.Episode.Season.ID, episode = xml.Scene.Episode.ID });
		}

	}
}
