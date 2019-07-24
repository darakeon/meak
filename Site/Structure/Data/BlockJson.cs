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
	public class BlockJson
	{
		private readonly String folderPath;
		private readonly String seasonID;
		private readonly String episodeID;
		private readonly String blockID;

		private Audio audio;

		public Block Block { get; set; }
		public FileInfo FileInfo { get; set; }

		public const String FIRST_BLOCK = "a";
		public const Int32 MINIMUM_PARAGRAPH_COUNT = 81;
		private readonly Boolean isAuthor = UrlUserType.IsAuthor();
		
		public BlockJson(String folderPath, String season, String episode)
			: this(folderPath, season, episode, FIRST_BLOCK) { }

		public BlockJson(String folderPath, String season, String episode, String block)
			: this(folderPath, season, episode, block, OpenEpisodeOption.GetCode) { }

		public BlockJson(String folderPath, String seasonID, String episodeID, String blockID, OpenEpisodeOption get)
		{
			this.folderPath = folderPath;
			this.seasonID = seasonID;
			this.episodeID = episodeID;
			this.blockID = blockID;

			var storyPath = Paths.BlockFilePath(folderPath, seasonID, episodeID, blockID);
			FileInfo = new FileInfo(storyPath);

			var episode = Episode.Get(folderPath, seasonID, episodeID);

			if (episode != null)
				populateBlock(get, episode);
		}

		#region For Constructor
		private void populateBlock(OpenEpisodeOption get, Episode episode)
		{
			Block = new Block
			{
				ID = FileInfo.NameWithoutExtension(),
				Episode = episode
			};

			audio = new Audio(folderPath, seasonID, episodeID, Block);

			if (get == OpenEpisodeOption.GetStory)
				readStory();
		}

		private void readStory()
		{
			var blockPart = FileInfo.FullName.Read<BlockPart>();

			verifyProperties(blockPart);
			adjustParagraphs(blockPart);

			foreach (var paragraph in blockPart.Paragraphs)
			{
				Block.ParagraphTypeList.Add(paragraph.Type);
				setText(paragraph);
			}

			audio.CopySongs();
		}

		private void adjustParagraphs(BlockPart blockPart)
		{
			if (isAuthor)
			{
				addParagraphsToLimit(blockPart);
			}
			else
			{
				blockPart.Paragraphs =
					blockPart.Paragraphs
						.Where(pg => pg.HasText)
						.ToList();
			}
		}

		private static void addParagraphsToLimit(BlockPart blockPart)
		{
			var begin = blockPart.Paragraphs.Count;
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

				blockPart.Paragraphs.Add(paragraph);
			}
		}

		private void setText(Paragraph paragraph)
		{
			switch (paragraph.Type)
			{
				case ParagraphType.Talk:
					var talk = ParagraphJson.GetTalk(paragraph);
					Block.TalkList.Add(talk);
					break;
				case ParagraphType.Teller:
					var teller = ParagraphJson.GetTeller(paragraph);
					Block.TellerList.Add(teller);
					break;
				case ParagraphType.Page:
					Block.PageList.Add(paragraph.Breaks);
					break;
			}
		}
		#endregion

		public void AddNewStory(String title)
		{
			FileInfo.CreateIfNotExists("{}");

			MainInfoJson.Save(
				title,
				"-",
				null,
				folderPath,
				seasonID,
				episodeID
			);

			fakeStory();

			WriteStory();
		}

		public void WriteStory()
		{
			var story = makeStory();
			var path = Paths.BlockFilePath(folderPath, seasonID, episodeID, blockID);
			path.Write(story);
		}

		private void fakeStory()
		{
			for (var j = 0; j < 3; j++)
			{
				Block.ParagraphTypeList.Add(ParagraphType.Teller);
				Block.TellerList.Add(tellerDefault());

				for (var k = 0; k < 5; k++)
				{
					Block.ParagraphTypeList.Add(ParagraphType.Talk);
					Block.TalkList.Add(talkDefault());
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

		private BlockPart makeStory()
		{
			var blockPart = FileInfo.FullName.Read<BlockPart>();

			blockPart.Paragraphs = new List<Paragraph>();

			verifyProperties(blockPart);

			var talkCounter = 0;
			var tellerCounter = 0;
			var pageCounter = 0;

			foreach (var paragraph in Block.ParagraphTypeList)
			{
				Paragraph child;

				switch (paragraph)
				{
					case ParagraphType.Talk:
						child = ParagraphJson.SetTalk(Block.TalkList[talkCounter]);
						talkCounter++;
						break;
					case ParagraphType.Teller:
						child = ParagraphJson.SetTeller(Block.TellerList[tellerCounter]);
						tellerCounter++;
						break;
					case ParagraphType.Page:
						child = ParagraphJson.SetPage(Block.PageList[pageCounter]);
						pageCounter++;
						break;
					default:
						throw new Exception($"Not recognized Paragraph [{paragraph}].");
				}

				if (child.HasText)
				{
					blockPart.Paragraphs.Add(child);
				}
			}

			return blockPart;
		}

		private void verifyProperties(BlockPart block)
		{
			if (block.Block != Block.ID)
				throw new Exception($"Block [{block.Block}] and file name [{Block.ID}] doesn't match.");

			if (block.Episode != Block.Episode.ID)
				throw new Exception($"Episode [{block.Episode}] and file path [{Block.Episode.ID}] doesn't match.");

			if (block.Season != Block.Episode.Season.ID)
				throw new Exception($"Season [{block.Season}] and file path [{Block.Episode.Season.ID}] doesn't match.");
		}
	}
}
