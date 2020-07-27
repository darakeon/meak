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
							processParagraph(paragraphType, block.TellerList[teller]);
							teller++;
							break;
						case ParagraphType.Talk:
							processParagraph(paragraphType, block.TalkList[talk]);
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
			Paragraph<T> paragraph
		) where T : struct
		{
			var lines = type == ParagraphType.Talk ? 1 : 0;

			var lineStart = type == ParagraphType.Talk ? 10 : 0;

			var lineSize = lineStart;

			foreach (var piece in paragraph.Pieces)
			{
				if (piece.Text == null)
					continue;

				var sizes = styles[type][piece.Style];

				var words = sizeWords(piece, sizes);
				piece.words = words;

				for (var w = 0; w < words.Count; w++)
				{
					var word = words[w];
					lineSize += word;

					if (lineSize > lineMaxSize)
					{
						lines++;
						piece.lineSizes.Add(lineSize - word);
						lineSize = word;
					}

					var isLast = w + 1 == words.Count;
					lineSize += isLast ? 0 : sizes[' '];
				}

				if (type == ParagraphType.Teller)
				{
					lines++;
					lineSize = lineStart;
				}
				piece.lineSizes.Add(lineSize);
			}

			paragraph.lines = lines;
		}

		private List<Int32> sizeWords<T>(Piece<T> piece, CharMap style)
			where T : struct
		{
			var words = new List<Int32>();
			var charSizes = 0;

			foreach (var character in piece.Text)
			{
				if (!style.Contains(character))
				{
					style.Add(character);
				}

				var size = style[character];

				if (character == ' ')
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
	}
}
