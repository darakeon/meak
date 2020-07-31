using System;
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
				var talk = 0;
				var teller = 0;

				for (var p = 0; p < block.ParagraphTypeList.Count; p++)
				{
					var type = block.ParagraphTypeList[p];
					Boolean pageAdded;

					switch (type)
					{
						case ParagraphType.Teller:
							addBlockSpace(p, b, block.TellerList[teller].Pieces[0].Style);

							pageAdded = processParagraph(
								type, p, block.TellerList[teller], block
							);

							teller++;
							break;
						case ParagraphType.Talk:
							addBlockSpace(p, b);

							pageAdded = processParagraph(
								type, p, block.TalkList[talk], block
							);

							talk++;
							break;
						case ParagraphType.Page:
						default:
							throw new NotImplementedException(
								$"{episode.Season}{episode}{block} [{p}]: {type}"
							);
					}

					if (pageAdded) p++;
				}
			}
		}

		private void addBlockSpace(int p, int b, TellerStyle? style = null)
		{
			var hasNoSpace = style != TellerStyle.Division
				&& style != TellerStyle.First;

			if (p == 0 && b != 0 && hasNoSpace)
				currentLine += 3;
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

		private List<Decimal> pieceWords<T>(ParagraphType type, Paragraph<T> paragraph) where T : struct
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

			var style = piece.Style.ToString().GetEnum<TalkStyle>();

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

		private void addTellerBreak<T>(ParagraphType type, Piece<T> piece) where T : struct
		{
			if (type != ParagraphType.Teller)
				return;

			currentLine++;

			if (piece.Style.Equals(TellerStyle.Division))
				currentLine += 4;

			if (piece.Style.Equals(TellerStyle.First))
				currentLine += 3;

			resetCurrentLineSize(type);
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
	}
}
