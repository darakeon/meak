using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Presentation.Models;
using Structure.Entities;
using Structure.Enums;
using Presentation.Helpers;
using Structure.Data;
using Structure.Helpers;


namespace Presentation.Controllers
{
    [StoriesAuthorize]
    public class SeasonController : Controller
    {
        #region INIT
        private Paths paths;
        private EpisodeXML episodeXML;
        
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            episodeXML = new EpisodeXML(Server);

            var xmlPath = episodeXML.PathXML;
            var cssPath = Server.MapPath("~/Assets/css");

            paths = new Paths(xmlPath, cssPath);
        }
        #endregion



        public ActionResult Index(String seasonID, String returnUrl)
        {
            return String.IsNullOrEmpty(seasonID)
                ? index(returnUrl)
                : episodes(seasonID);
        }

        private ActionResult index(String returnUrl)
        {
            var model = new SeasonIndexModel(paths)
                            {
                                LogOn =
                                    {
                                        Is = !String.IsNullOrEmpty(returnUrl),
                                        ReturnUrl = returnUrl
                                    }
                            };

            var introNum = new Random().Next(1, 3);

            return View("Intro" + introNum, model);
        }

        private ActionResult episodes(String seasonID)
        {
            var model = new SeasonSeasonModel(paths, seasonID);

            return View("Season", model);
        }




        public ActionResult Episode(String seasonID, String episodeID)
        {
            Episode episode;

            try
            {
                episode = episodeXML.GetEpisode(seasonID, episodeID);
            }
            catch (Exception e)
            {
                return View("Error", new ErrorModel(paths) { Message = e.Message });
            }


            var model = new SeasonEditEpisodeModel(paths) { Story = episode };


            model.GetSuggestionLists();


            if (UrlUserType.IsAuthor())
            {
                model.TabIndex = 1;
                return editEpisode(model);
            }

            return View(model);
        }
        
        private ActionResult editEpisode(SeasonEpisodeModel model)
        {
            EpisodeEditionHelper.PutCharacter(model.Story);

            return View("Author/Episode", model);
        }



        [HttpPost]
        public ActionResult Episode(SeasonEpisodeModel model, String seasonID, String episodeID, String sceneID)
        {
            var xml = new SceneXML(paths.Xml, seasonID, episodeID, sceneID) {Scene = model.Story[sceneID]};

            EpisodeEditionHelper.CutCharacter(xml.Scene);

            xml.WriteStory();

            return RedirectToAction("Episode", new { season = seasonID, episode = episodeID });
        }



        [HttpPost]
        public ActionResult Adder(String scene, String type, String subtype, Int32? paragraph, Int32? teller, Int32? talk, Int32? piece)
        {
            var adder = new Adder(paths);

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
            var model = new SeasonAddEpisodeModel(paths);

            return View("Author/AddEpisode", model);
        }

        [HttpPost]
        public ActionResult AddEpisode(SeasonAddEpisodeModel model)
        {
            if (!model.SeasonEpisode.IsValid())
            {
                return RedirectToAction("AddEpisode");
            }


            var xml = new SceneXML(paths.Xml, model.SeasonEpisode.Season, model.SeasonEpisode.Episode);

            xml.AddNewStory(model.Title);


            return RedirectToAction("Episode", new { season = xml.Scene.Episode.Season.ID, episode = xml.Scene.Episode.ID });
        }

    }
}
