using System;
using System.Collections.Generic;
using Structure.Entities.System;
using Structure.Enums;

namespace Structure.Printer
{
	public class Printer
	{
		private static ParagraphMap styles;

		private const Int32 pageLines = 30;
		private const Int32 lineMaxSize = 306;
		private const Int32 spaceSize = 4;
		private const Int32 dashSize = 10;

		private Int32 currentLineSize;
		private Int32 currentLine = 1;

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
			foreach (var block in episode.BlockList)
			{
				var talk = 0;
				var teller = 0;

				for (var p = 0; p < block.ParagraphTypeList.Count; p++)
				{
					var paragraphType = block.ParagraphTypeList[p];

					switch (paragraphType)
					{
						case ParagraphType.Teller:
							processParagraph(paragraphType, ref p, block.TellerList[teller], block);
							teller++;
							break;
						case ParagraphType.Talk:
							processParagraph(paragraphType, ref p, block.TalkList[talk], block);
							talk++;
							break;
						case ParagraphType.Page:
						default:
							throw new NotImplementedException($"{episode.Season}{episode}{block} [{p}]: {paragraphType}");
					}
				}
			}
		}

		private void processParagraph<T>(
			ParagraphType type,
			ref Int32 position,
			Paragraph<T> paragraph,
			Block block
		) where T : struct
		{
			resetCurrentLineSize(type);

			var words = pieceWords(type, paragraph);

			if (paragraph is Talk talk)
			{
				var character = $"({talk.Character})";
				var @default = styles[type][TalkStyle.Default];

				words.AddRange(sizeWords(character, @default));

				currentLine++;
			}

			wordsToLines(words);

			paragraph.DebugLines = currentLine;

			if (currentLine >= pageLines)
			{
				var insertAt = currentLine == pageLines
					? position + 1
					: position;

				currentLine -= pageLines;

				position++;

				block.ParagraphTypeList.Insert(insertAt, ParagraphType.Page);
			}

		}

		private void resetCurrentLineSize(ParagraphType type)
		{
			currentLineSize = type == ParagraphType.Talk ? dashSize : 0;
		}

		private List<Int32> pieceWords<T>(ParagraphType type, Paragraph<T> paragraph) where T : struct
		{
			var result = new List<Int32>();

			foreach (var piece in paragraph.Pieces)
			{
				if (piece.Text == null)
					continue;

				var sizes = styles[type][piece.Style];

				var words = sizeWords(piece, sizes);
				piece.DebugWords = words;

				result.AddRange(words);

				if (type == ParagraphType.Teller)
				{
					currentLine++;

					if (piece.Style.Equals(TellerStyle.Division))
					{
						currentLine += 4;
					}

					if (piece.Style.Equals(TellerStyle.First))
					{
						currentLine += 3;
					}

					resetCurrentLineSize(type);
				}
			}

			return result;
		}

		private List<Int32> sizeWords<T>(Piece<T> piece, CharMap sizes) where T : struct
		{
			var text = piece.Text;

			if (piece.Style.Equals(TalkStyle.Teller))
			{
				text = $"– {text} –";
			}

			return sizeWords(text, sizes);
		}

		private List<Int32> sizeWords(String text, CharMap style)
		{
			var words = new List<Int32>();
			var charSizes = 0;

			foreach (var character in text)
			{
				if (!style.Contains(character))
				{
					style.Add(character);
				}

				var size = style[character];

				if (character == ' ' || character == '-')
				{
					words.Add(charSizes);
					charSizes = 0;
				}
				else
				{
					charSizes += size;
				}
			}

			words.Add(charSizes);

			return words;
		}

		private void wordsToLines(List<Int32> words)
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
	}
}
