using System;
using System.Collections.Generic;
using System.Linq;
using Structure.Entities.System;
using Structure.Enums;

namespace Structure.Printer
{
	public class CharsEpisode : Episode
	{
		public CharsEpisode()
		{
			var block = new Block{ ID = "a" };

			foreach (var paragraph in Printer.chars())
			{
				switch (paragraph.Key)
				{
					case ParagraphType.Talk:
						addPieces<Talk, TalkStyle>(
							paragraph.Value,
							ParagraphType.Talk,
							block.TalkList,
							block.ParagraphTypeList
						);
						break;
					case ParagraphType.Teller:
						addPieces<Teller, TellerStyle>(
							paragraph.Value,
							ParagraphType.Teller,
							block.TellerList,
							block.ParagraphTypeList
						);
						break;
					case ParagraphType.Page:
					default:
						throw new NotImplementedException();
				}
			}

			BlockList = new List<Block> {block};
		}

		private static void addPieces<P, S>(
			StyleMap styleMap,
			ParagraphType type,
			IList<P> paragraphList,
			IList<ParagraphType> paragraphTypeList
		)
			where P : Paragraph<S>, new()
			where S : struct
		{
			foreach (var style in styleMap)
			{
				var styleEnum = (S) style.Key;

				foreach (var @char in style.Value.Where(c => c.Value == 0))
				{
					var paragraph = new P();
					var piece = new Piece<S>
					{
						Style = styleEnum,
						Text = "|| ||"
					};

					piece.Text = piece.Text.PadLeft(15, @char.Key);

					paragraph.Pieces.Add(piece);
					paragraphList.Add(paragraph);
					paragraphTypeList.Add(type);
				}
			}
		}
	}
}
