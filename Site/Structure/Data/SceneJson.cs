using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Structure.Entities.Json;
using Structure.Entities.System;
using Structure.Enums;
using Structure.Extensions;
using Structure.Helpers;

namespace Structure.Data
{
	public class SceneJson
	{
		private readonly string folderPath;
		private readonly string seasonID;
		private readonly string episodeID;
		private readonly string sceneID;

		public Scene Scene { get; set; }
		public FileInfo FileInfo { get; set; }

		public const String FIRST_SCENE = "a";
		public const Int32 MINIMUM_PARAGRAPH_COUNT = 81;
		private readonly Boolean isAuthor = UrlUserType.IsAuthor();
		
		public SceneJson(String folderPath, String season, String episode)
			: this(folderPath, season, episode, FIRST_SCENE) { }

		public SceneJson(String folderPath, String season, String episode, String scene)
			: this(folderPath, season, episode, scene, OpenEpisodeOption.GetCode) { }

		public SceneJson(String folderPath, String seasonID, String episodeID, String sceneID, OpenEpisodeOption get)
		{
			this.folderPath = folderPath;
			this.seasonID = seasonID;
			this.episodeID = episodeID;
			this.sceneID = sceneID;

			var storyPath = Paths.SceneFilePath(folderPath, seasonID, episodeID, sceneID);
			FileInfo = new FileInfo(storyPath);

			var episode = new Episode(folderPath, seasonID, episodeID);
			populateScene(get, episode);
		}

		#region For Constructor
		private void populateScene(OpenEpisodeOption get, Episode episode)
		{
			Scene = new Scene
			{
				ID = FileInfo.NameWithoutExtension(),
				Episode = episode
			};

			if (get == OpenEpisodeOption.GetStory)
				readStory();
		}

		private void readStory()
		{
			var scenePart = FileInfo.FullName.Read<ScenePart>();

			verifyProperties(scenePart);
			adjustParagraphs(scenePart);

			foreach (var paragraph in scenePart.Paragraphs)
			{
				Scene.ParagraphTypeList.Add(paragraph.Type);
				setText(paragraph);
			}
		}

		private void adjustParagraphs(ScenePart scenePart)
		{
			if (isAuthor)
			{
				addParagraphsToLimit(scenePart);
			}
			else
			{
				scenePart.Paragraphs =
					scenePart.Paragraphs
						.Where(pg => pg.HasText)
						.ToList();
			}
		}

		private static void addParagraphsToLimit(ScenePart scenePart)
		{
			var begin = scenePart.Paragraphs.Count;
			var end = MINIMUM_PARAGRAPH_COUNT;

			for (var p = begin; p < end; p++)
			{
				var piece = new Piece
				{
					Type = TalkStyle.Default.ToString()
				};

				var paragraph = new Paragraph
				{
					Type = ParagraphType.Talk,
					Pieces = new[] { piece }
				};

				scenePart.Paragraphs.Add(paragraph);
			}
		}

		private void setText(Paragraph paragraph)
		{
			switch (paragraph.Type)
			{
				case ParagraphType.Talk:
					var talk = ParagraphJson.GetTalk(paragraph);
					Scene.TalkList.Add(talk);
					break;
				case ParagraphType.Teller:
					var teller = ParagraphJson.GetTeller(paragraph);
					Scene.TellerList.Add(teller);
					break;
			}
		}
		#endregion

		public void AddNewStory(String title)
		{
			FileInfo.CreateIfNotExists("{}");

			MainInfoJson.Save(title, "Story summary.", folderPath, seasonID, episodeID);

			fakeStory();

			WriteStory();
		}

		public void WriteStory()
		{
			var story = makeStory();
			var path = Paths.SceneFilePath(folderPath, seasonID, episodeID, sceneID);
			path.Write(story);
		}

		private void fakeStory()
		{
			for (var j = 0; j < 3; j++)
			{
				Scene.ParagraphTypeList.Add(ParagraphType.Teller);
				Scene.TellerList.Add(tellerDefault());

				for (var k = 0; k < 5; k++)
				{
					Scene.ParagraphTypeList.Add(ParagraphType.Talk);
					Scene.TalkList.Add(talkDefault());
				}
			}
		}

		private static Teller tellerDefault()
		{
			return new Teller
			{
				Pieces = {
					new Piece<TellerStyle> { Style = TellerStyle.Default, Text = "_" }
				}
			};
		}

		private static Talk talkDefault()
		{
			return new Talk
			{
				Pieces = {
					new Piece<TalkStyle> { Style = TalkStyle.Default, Text = "_" }
				},
				Character = "Keon"
			};
		}

		private ScenePart makeStory()
		{
			var scenePart = FileInfo.FullName.Read<ScenePart>();

			scenePart.Paragraphs = new List<Paragraph>();

			verifyProperties(scenePart);

			var talkCounter = 0;
			var tellerCounter = 0;

			foreach (var paragraph in Scene.ParagraphTypeList)
			{
				Paragraph child;

				switch (paragraph)
				{
					case ParagraphType.Talk:
						child = ParagraphJson.SetTalk(Scene.TalkList[talkCounter]);
						talkCounter++;
						break;
					case ParagraphType.Teller:
						child = ParagraphJson.SetTeller(Scene.TellerList[tellerCounter]);
						tellerCounter++;
						break;
					default:
						throw new Exception($"Not recognized Paragraph [{paragraph}].");
				}

				if (child.HasText)
				{
					scenePart.Paragraphs.Add(child);
				}
			}

			return scenePart;
		}

		private void verifyProperties(ScenePart scene)
		{
			if (scene.Scene != Scene.ID)
				throw new Exception($"Scene [{scene.Scene}] and file name [{Scene.ID}] doesn't match.");

			if (scene.Episode != Scene.Episode.ID)
				throw new Exception($"Episode [{scene.Episode}] and file path [{Scene.Episode.ID}] doesn't match.");

			if (scene.Season != Scene.Episode.Season.ID)
				throw new Exception($"Season [{scene.Season}] and file path [{Scene.Episode.Season.ID}] doesn't match.");
		}




	}
}
