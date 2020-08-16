using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Structure.Entities.System;
using Structure.Enums;
using Structure.Extensions;

namespace Structure.Printer
{
	public class Printer
	{
		private static ParagraphMap styles;

		private const Int32 pageLines = 30;
		private const Decimal lineMaxSize = 306.3m;

		private readonly IDictionary<ParagraphType, Decimal> spaceSize =
			new Dictionary<ParagraphType, Decimal>
			{
				{ ParagraphType.Talk, 3.1m },
				{ ParagraphType.Teller, 3.2m }
			};

		private const Decimal dashSize = 11;

		private Int32 currentLine = 1;
		private Int32 oldLine;

		static Printer()
		{
			initialize();
		}

		public static ParagraphMap chars()
		{
			return styles;
		}

		private static void initialize()
		{
			styles = new ParagraphMap();
			styles.Add<TalkStyle>(ParagraphType.Talk);
			styles.Add<TellerStyle>(ParagraphType.Teller);
		}

		public static void Paginate(Episode episode)
		{
			new Printer(episode).paginate();
		}

		private readonly Episode episode;

		private Printer(Episode episode)
		{
			this.episode = episode;
		}

		private void paginate()
		{
			var path = $"{episode.Season}{episode.ID}.log";
			var metricsExists = File.Exists(path);

			if (!metricsExists)
				File.WriteAllText(path, "");

			var lines = File.ReadAllLines(path)
				.Select(Int32.Parse)
				.ToList();
			var line = 0;

			for (var b = 0; b < episode.BlockList.Count; b++)
			{
				var block = episode.BlockList[b];
				var talkIndex = 0;
				var tellerIndex = 0;

				for (var p = 0; p < block.ParagraphTypeList.Count; p++)
				{
					oldLine = currentLine;

					var type = block.ParagraphTypeList[p];

					Boolean pageAdded;
					Boolean blockSpaceAdded;
					Action<Int32> removeLines;
					Func<Int32> getLines;

					switch (type)
					{
						case ParagraphType.Teller:
							var teller = block.TellerList[tellerIndex];
							removeLines = n => teller.DebugLines -= n;
							getLines = () => teller.DebugLines;

							var style = teller.Pieces[0].Style;
							blockSpaceAdded = addBlockSpace(p, b, style);

							pageAdded = processParagraph(type, p, teller, block);

							tellerIndex++;
							break;
						case ParagraphType.Talk:
							var talk = block.TalkList[talkIndex];
							removeLines = n => talk.DebugLines -= n;
							getLines = () => talk.DebugLines;

							blockSpaceAdded = addBlockSpace(p, b);

							pageAdded = processParagraph(type, p, talk, block);

							talkIndex++;
							break;
						case ParagraphType.Page:
						default:
							throw new NotImplementedException(
								$"{episode.Season}{episode}{block} [{p}]: {type}"
							);
					}

					if (pageAdded)
					{
						block.PageCount++;

						if (blockSpaceAdded)
						{
							removeLines(3);
							currentLine -= 3;
						}
						else
						{
							currentLine -= linesToRemove(block, p);
						}

						p++;
					}

					if (!metricsExists)
					{
						File.AppendAllText(path, $"{getLines()}\n");
					}
					else if (lines[line] != getLines())
					{
						removeLines(getLines() * 2);
					}

					line++;
				}
			}
		}

		private Boolean addBlockSpace(Int32 paragraph, Int32 block, TellerStyle? style = null)
		{
			var hasNoSpace = style != TellerStyle.Division
				&& style != TellerStyle.First;

			var add = paragraph == 0 && block != 0 && hasNoSpace;

			if (add)
				currentLine += 3;

			return add;
		}

		private Boolean processParagraph<T>(
			ParagraphType type,
			Int32 position,
			Paragraph<T> paragraph,
			Block block
		) where T : struct
		{
			var words = pieceWords(type, paragraph);

			addTalkBreakAndCharacter(type, paragraph, words);

			wordsToLines(words, type);

			paragraph.DebugLines = currentLine - oldLine;

			var addPage = currentLine >= pageLines;

			if (addPage)
				addPageBreak(position, block);

			return addPage;
		}

		private List<List<Decimal>> pieceWords<T>(
			ParagraphType type,
			Paragraph<T> paragraph
		) where T : struct
		{
			var result = new List<List<Decimal>>();

			foreach (var piece in paragraph.Pieces)
			{
				if (piece.Text == null)
					continue;

				var sizes = styles[type][piece.Style];

				var text = transformToShown(piece, piece.Text);
				var words = sizeWords(text, sizes);
				piece.DebugWords = words;

				result.Add(words);

				addTellerBreak(type, piece);
			}

			return result;
		}

		private String transformToShown<T>(Piece<T> piece, String text) where T : struct
		{
			if (typeof(T) == typeof(TalkStyle))
			{
				var style = piece.Style.GetEnum<TalkStyle>();

				// ReSharper disable once SwitchStatementMissingSomeCases
				switch (style)
				{
					case TalkStyle.Teller:
						return $"– {text} –";

					case TalkStyle.Read:
						return $"[{text}]";

					case TalkStyle.Thought:
						return $"(({text}))";

					case TalkStyle.Translate:
						return $"{{{text}}}";

					case TalkStyle.Screamed:
						return text.ToUpper();

					default:
						return text;
				}
			}

			if (typeof(T) == typeof(TellerStyle))
			{
				var style = piece.Style.GetEnum<TellerStyle>();

				if (style == TellerStyle.Division)
					return text.ToUpper();
			}

			return text;
		}

		private List<Decimal> sizeWords(String text, CharMap style)
		{
			var words = new List<Decimal>();
			var charSizes = Decimal.Zero;

			foreach (var character in text)
			{
				if (character == ' ' || character == '-')
				{
					words.Add(charSizes);
					charSizes = 0;
				}
				else
				{
					if (!style.Contains(character))
						style.Add(character);

					charSizes += style[character];
				}
			}

			words.Add(charSizes);

			return words;
		}

		private void addTellerBreak<T>(
			ParagraphType type,
			Piece<T> piece
		) where T : struct
		{
			if (type != ParagraphType.Teller)
				return;

			currentLine++;

			var style = piece.Style.GetEnum<TellerStyle>();
			currentLine += linesToAdd(style);
		}

		private Int32 linesToAdd(TellerStyle style)
		{
			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (style)
			{
				case TellerStyle.Division:
					return 4;
				case TellerStyle.First:
					return 3;
				default:
					return 0;
			}
		}

		private void addTalkBreakAndCharacter<T>(
			ParagraphType type,
			Paragraph<T> paragraph,
			List<List<Decimal>> words
		) where T : struct
		{
			if (!(paragraph is Talk talk))
				return;

			var character = $"({talk.Character})";
			var @default = styles[type][TalkStyle.Default];

			var name = sizeWords(character, @default);

			words.Add(name);

			talk.DebugCharacter = name;

			currentLine++;
		}

		private void wordsToLines(List<List<Decimal>> words, ParagraphType type)
		{
			var currentLineSize = type == ParagraphType.Talk ? dashSize : 0;

			for (var s = 0; s < words.Count; s++)
			{
				for (var w = 0; w < words[s].Count; w++)
				{
					var word = words[s][w];
					currentLineSize += word;

					if (currentLineSize > lineMaxSize)
					{
						currentLine++;
						currentLineSize = word;
					}

					var lastWord = w + 1 == words[s].Count;
					var lastSentence = s + 1 == words.Count;
					var isTalk = type == ParagraphType.Talk;
					var isLast = lastWord && (!isTalk || lastSentence);

					currentLineSize += isLast ? 0 : spaceSize[type];
				}

				if (type == ParagraphType.Teller)
					currentLineSize = 0;
			}
		}

		private void addPageBreak(Int32 position, Block block)
		{
			var insertAt = currentLine == pageLines
				? position + 1
				: position;

			currentLine -= currentLine > pageLines
				? oldLine
				: pageLines;

			block.ParagraphTypeList.Insert(insertAt, ParagraphType.Page);
		}

		private Int32 linesToRemove(Block block, Int32 paragraph)
		{
			var type = block.ParagraphTypeList[paragraph];

			var firstIndex =
				type == ParagraphType.Page
					? paragraph + 1
					: paragraph + 2;

			var paragraphsCount = block.ParagraphTypeList.Count;

			if (firstIndex >= paragraphsCount)
			{
				var blockIndex = episode.BlockList.IndexOf(block);

				if (blockIndex >= episode.BlockList.Count)
					return 0;

				block = episode.BlockList[blockIndex + 1];
				firstIndex -= paragraphsCount;
			}

			var firstParagraph =
				block.ParagraphTypeList[firstIndex];

			if (firstParagraph == ParagraphType.Talk)
				return 0;

			var tellerIndex = getIndex(block, firstIndex, ParagraphType.Teller);
			var teller = block.TellerList[tellerIndex];

			var remove = linesToAdd(teller.Pieces[0].Style);

			teller.DebugLines -= remove;

			return remove;
		}

		private Int32 getIndex(Block block, Int32 until, ParagraphType type)
		{
			return block.ParagraphTypeList
		       .Take(until + 1)
		       .Count(p => p == type) - 1;
		}
	}
}
