using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using Presentation.Models;
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
        private BaseModel.Paths paths;
        
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var folder = ConfigurationManager.AppSettings["Path"];

            if (folder == null)
                throw new Exception("XML Path not configured.");


            var xmlPath = 
                folder.Substring(1, 1) == ":"
                    ? folder
                    : Path.Combine(Server.MapPath("~"), folder);


            if (!Directory.Exists(xmlPath))
                throw new Exception(String.Format("Path '{0}' doesn't exists.", xmlPath));



            var cssPath = Server.MapPath("~/Assets/css");


            paths = new BaseModel.Paths(xmlPath, cssPath);
        }
        #endregion



        public ActionResult Index(String season, String returnUrl)
        {
            return String.IsNullOrEmpty(season)
                ? Index(returnUrl)
                : Season(season);
        }

        public ActionResult Index(String returnUrl)
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

        public ActionResult Season(String season)
        {
            var model = new SeasonSeasonModel(paths, season);

            return View("Season", model);
        }




        public ActionResult Episode(String season, String episode)
        {
            EpisodeXML xml;


            try
            {
                xml = new EpisodeXML(paths.Xml, season, episode, eOpenEpisodeOption.getStory);
            }
            catch (FileNotFoundException)
            {
                return View("Error", new ErrorModel(paths) { Message = "Temporada e/ou Capítulo não encontrado(s)." });
            }
            catch (Exception e)
            {
                return View("Error", new ErrorModel(paths) { Message = e.Message });
            }


            var model = new SeasonEditEpisodeModel(paths) { Story = xml.Episode };


            EpisodeNavigation.SetNavigation(model, paths.Xml);
            
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
        public ActionResult Episode(SeasonEpisodeModel model, String season, String episode)
        {
            var xml = new EpisodeXML(paths.Xml, season, episode) {Episode = model.Story};

            EpisodeEditionHelper.CutCharacter(xml.Episode);

            xml.WriteStory();

            return RedirectToAction("Episode", new { season, episode });
        }



        [HttpPost]
        public ActionResult Adder(String type, String subtype, Int32? paragraph, Int32? teller, Int32? talk, Int32? piece)
        {
            var adder = new Adder(paths);

            switch ((type + subtype).ToLower())
            {
                case "pieceteller":
                    adder.SetPieceTeller(piece ?? 0, teller ?? 0, talk ?? 0);
                    break;
                case "piecetalk":
                    adder.SetPieceTalk(piece ?? 0, talk ?? 0, teller ?? 0);
                    break;
                case "paragraphteller":
                    adder.SetParagraphTeller(paragraph ?? 0, teller ?? 0, talk ?? 0);
                    break;
                case "paragraphtalk":
                    adder.SetParagraphTalk(paragraph ?? 0, talk ?? 0, teller ?? 0);
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


            var xml = new EpisodeXML(paths.Xml, model.SeasonEpisode.Season, model.SeasonEpisode.Episode);

            xml.AddNewStory(model.Title);

            return RedirectToAction("Episode", new { season = xml.Episode.Season.ID, episode = xml.Episode.ID });
        }
    }
}
