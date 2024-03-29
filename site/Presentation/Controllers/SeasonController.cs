﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Helpers;
using Structure.Data;
using Structure.Helpers;
using Structure.Printer;

namespace Presentation.Controllers
{
	public class SeasonController : Controller
	{
		public ActionResult Index(String seasonID)
		{
			return String.IsNullOrEmpty(seasonID)
				? index()
				: episodes(seasonID);
		}

		private ActionResult index()
		{
			var model = new BaseModel();
			return View("Intro", model);
		}

		private ActionResult episodes(String seasonID)
		{
			var model = new SeasonSeasonModel(seasonID);

			if (model.EpisodeList.Any())
				return View("Summary", model);

			var route = RouteStories.With(seasonID, "01");
			return Redirect(route.ToString());
		}

		public ActionResult Episode(
			String seasonID,
			String episodeID,
			String blockID,
			AuthorMode? show
		)
		{
			SeasonEditEpisodeModel model;

			try
			{
				model = new SeasonEditEpisodeModel(
					seasonID,
					episodeID,
					blockID,
					show
				);
			}
			catch (Exception e)
			{
				if (Config.IsAuthor)
					throw;

				return View("Error", new ErrorModel
				{
					Error = e
				});
			}

			if (model.Story == null)
			{
				return Redirect("/");
			}

			model.GetSuggestionLists();

			if (Config.IsAuthor)
			{
				model.Fix();
				model.TabIndex = 1;
				return View("Author/Episode", model);
			}

			return View(model);
		}

		public ActionResult Print()
		{
			var model = new SeasonEditEpisodeModel
			{
				Story = new CharsEpisode(),
				ReadingBlock = "a"
			};

			return View("Episode", model);
		}

		[StoriesAuthorize, HttpPost]
		public void EditTitle(SeasonEpisodeModel model, String seasonID, String episodeID)
		{
			MainInfoJson.Save(
				model.Story.Title,
				model.Story.Synopsis,
				model.Story.Summary,
				model.Paths.Json,
				seasonID,
				episodeID
			);
		}

		[StoriesAuthorize, HttpPost]
		public void EditBlock(SeasonEpisodeModel model, String seasonID, String episodeID, String blockID)
		{
			var json = new BlockJson(
				model.Paths.Json, seasonID, episodeID, blockID
				)
			{
				Block = model.Story[blockID]
			};

			json.WriteStory();
		}

		[StoriesAuthorize, HttpPost]
		public ActionResult Adder(Int32 block, String type, String subtype, Int32? paragraph, Int32? teller, Int32? talk, Int32? page, Int32? piece)
		{
			var adder = new Adder();

			adder.SetBlock(block);

			switch ((type + subtype).ToLower())
			{
				case "pieceteller":
					adder.SetPieceTeller(block, piece ?? 0, teller ?? 0, talk ?? 0);
					break;
				case "piecetalk":
					adder.SetPieceTalk(block, piece ?? 0, talk ?? 0, teller ?? 0);
					break;
				case "paragraphteller":
					adder.SetParagraphTeller(block, paragraph ?? 0, teller ?? 0, talk ?? 0, page ?? 0);
					break;
				case "paragraphtalk":
					adder.SetParagraphTalk(block, paragraph ?? 0, teller ?? 0, talk ?? 0, page ?? 0);
					break;
				case "paragraphpage":
					adder.SetParagraphPage(block, paragraph ?? 0, teller ?? 0, talk ?? 0, page ?? 0);
					break;
				default:
					throw new Exception("Unknown Adder Type.");
			}

			return PartialView(adder.View, adder.Model);
		}

		[StoriesAuthorize]
		public ActionResult AddEpisode()
		{
			var model = new SeasonAddEpisodeModel();

			return View("Author/AddEpisode", model);
		}

		[StoriesAuthorize, HttpPost]
		public ActionResult AddEpisode(SeasonAddEpisodeModel model)
		{
			if (!model.SeasonEpisode.IsValid())
			{
				return RedirectToAction("AddEpisode");
			}


			var json = new BlockJson(
				model.Paths.Json, 
				model.SeasonEpisode.Season, 
				model.SeasonEpisode.Episode
			);

			json.AddNewStory(model.Title);

			return RedirectToAction("Episode", new
			{
				seasonID = json.Block.Episode.Season.ID,
				episodeID = json.Block.Episode.ID
			});
		}
	}
}
