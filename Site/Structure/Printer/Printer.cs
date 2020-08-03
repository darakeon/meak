using System;
using System.Linq;
using System.Collections.Generic;
using Structure.Entities.System;
using Structure.Enums;
using Structure.Extensions;

namespace Structure.Printer
{
	public class Printer
	{
		private static ParagraphMap styles;

		private const Int32 pageLines = 30;
		private const Int32 lineMaxSize = 306;
		private const Int32 spaceSize = 4;
		private const Int32 dashSize = 10;

		private Decimal currentLineSize;
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
			for (var b = 0; b < episode.BlockList.Count; b++)
			{
				var block = episode.BlockList[b];
				var talkIndex = 0;
				var tellerIndex = 0;

				for (var p = 0; p < block.ParagraphTypeList.Count; p++)
				{
					var type = block.ParagraphTypeList[p];

					Boolean pageAdded;
					Boolean blockSpaceAdded;

					switch (type)
					{
						case ParagraphType.Teller:
							var teller = block.TellerList[tellerIndex];

							var style = teller.Pieces[0].Style;
							blockSpaceAdded = addBlockSpace(p, b, style);

							pageAdded = processParagraph(type, p, teller, block);

							tellerIndex++;
							break;
						case ParagraphType.Talk:
							var talk = block.TalkList[talkIndex];

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
						currentLine -= linesToRemove(block, p, blockSpaceAdded);
						p++;
					}
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
			oldLine = currentLine;

			resetCurrentLineSize(type);

			var words = pieceWords(type, paragraph);

			addTalkBreakAndCharacter(type, paragraph, words);

			wordsToLines(words);

			var addPage = currentLine >= pageLines;

			if (addPage)
				addPageBreak(position, block);

			paragraph.DebugLines = currentLine;
			return addPage;
		}

		private void resetCurrentLineSize(ParagraphType type)
		{
			currentLineSize = type == ParagraphType.Talk ? dashSize : 0;
		}

		private List<Decimal> pieceWords<T>(
			ParagraphType type,
			Paragraph<T> paragraph
		) where T : struct
		{
			var result = new List<Decimal>();

			foreach (var piece in paragraph.Pieces)
			{
				if (piece.Text == null)
					continue;

				var sizes = styles[type][piece.Style];

				var text = addSurround(piece, piece.Text);
				var words = sizeWords(text, sizes);
				piece.DebugWords = words;

				result.AddRange(words);

				addTellerBreak(type, piece);
			}

			return result;
		}

		private String addSurround<T>(Piece<T> piece, String text) where T : struct
		{
			if (typeof(T) != typeof(TalkStyle))
				return text;

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

				default:
					return text;
			}
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

			resetCurrentLineSize(type);
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
			List<Decimal> words
		) where T : struct
		{
			if (!(paragraph is Talk talk))
				return;

			var character = $"({talk.Character})";
			var @default = styles[type][TalkStyle.Default];

			var name = sizeWords(character, @default);

			words.AddRange(name);

			talk.DebugCharacter = name;

			currentLine++;
		}

		private void wordsToLines(List<Decimal> words)
		{
			for (var w = 0; w < words.Count; w++)
			{
				var word = words[w];
				currentLineSize += word;

				if (currentLineSize > lineMaxSize)
				{
					currentLine++;
					currentLineSize = word;
				}

				var isLast = w + 1 == words.Count;
				currentLineSize += isLast ? 0 : spaceSize;
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

		private Int32 linesToRemove(Block block, Int32 paragraph, Boolean blockSpaceAdded)
		{
			if (blockSpaceAdded)
				return 3;

			var firstIndex =
				block.ParagraphTypeList[paragraph] == ParagraphType.Page
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
