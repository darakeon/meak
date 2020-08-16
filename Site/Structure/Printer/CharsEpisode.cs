using System;
using System.Collections.Generic;
using System.Linq;
using Structure.Entities.System;
using Structure.Enums;

namespace Structure.Printer
{
	public class CharsEpisode : Episode
	{
		private readonly Block block;

		public CharsEpisode()
		{
			block = new Block{ ID = "a" };

			foreach (var paragraph in Printer.chars())
			{
				switch (paragraph.Key)
				{
					case ParagraphType.Talk:
						addPieces<Talk, TalkStyle>(
							paragraph.Value,
							ParagraphType.Talk,
							block.TalkList
						);
						break;
					case ParagraphType.Teller:
						addPieces<Teller, TellerStyle>(
							paragraph.Value,
							ParagraphType.Teller,
							block.TellerList
						);
						break;
					case ParagraphType.Page:
					default:
						throw new NotImplementedException();
				}
			}

			BlockList = new List<Block> {block};
		}

		private void addPieces<P, S>(
			StyleMap styleMap,
			ParagraphType type,
			IList<P> paragraphList
		)
			where P : Paragraph<S>, new()
			where S : struct
		{
			foreach (var style in styleMap)
			{
				var missingSizes = style.Value
					.Where(c => c.Value == 0)
					.ToList();

				if (missingSizes.Any())
				{
					addParagraph(
						type,
						paragraphList,
						(S)style.Key,
						$"{type}_{style.Key}"
					);
				}

				foreach (var @char in missingSizes)
				{
					addParagraph(
						type,
						paragraphList,
						(S) style.Key,
						"".PadLeft(20, @char.Key)
					);
				}
			}
		}

		private void addParagraph<P, S>(ParagraphType type, IList<P> paragraphList, S style, String text)
			where P : Paragraph<S>, new() where S : struct
		{
			var paragraph = new P();
			var piece = new Piece<S>
			{
				Style = style,
				Text = text,
			};

			paragraph.Pieces.Add(piece);
			paragraphList.Add(paragraph);
			block.ParagraphTypeList.Add(type);
		}
	}
}
