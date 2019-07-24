using System;
using System.Collections.Generic;
using System.Linq;
using Structure.Entities.Json;
using Structure.Entities.System;
using Structure.Enums;
using Structure.Extensions;
using Structure.Helpers;

namespace Structure.Data
{
	public class ParagraphJson
	{
		public static Talk GetTalk(Paragraph paragraph)
		{
			var talk = getParagraph<Talk ,TalkStyle>(paragraph);

			var character = paragraph.Character;

			var isAuthor = UrlUserType.IsAuthor();

			
			if (String.IsNullOrEmpty(character) && !isAuthor)
				throw new Exception($"No Character: {talk}");
			

			talk.Character = character;

			return talk;
		}

		public static Teller GetTeller(Paragraph paragraph)
		{
			return getParagraph<Teller, TellerStyle>(paragraph);
		}

		private static TP getParagraph<TP, TS>(Paragraph genericParagraph)
			where TP : Paragraph<TS>, new()
			where TS : struct
		{
			var paragraph = new TP();

			foreach (var genericPiece in genericParagraph.Pieces)
			{
				var piece = PieceJson<TS>.GetPiece(
					genericPiece, 
					genericParagraph.Type
				);

				paragraph.Pieces.Add(piece);
			}

			return paragraph;
		}

		public static Paragraph SetTalk(Talk talk)
		{
			var node = setParagraph(talk);
			node.Character = talk.Character;
			return node;
		}

		public static Paragraph SetTeller(Teller teller)
		{
			return setParagraph(teller);
		}

		public static Paragraph SetPage(Int32? page)
		{
			return new Paragraph
			{
				Type = ParagraphType.Page,
				Breaks = page
			};
		}

		private static Paragraph setParagraph<T>(Paragraph<T> paragraph) where T : struct
		{
			return new Paragraph
			{
				Type = getType<T>(),
				Pieces = getPieces(paragraph)
			};
		}

		private static ParagraphType getType<T>() where T : struct
		{
			return typeof(T).Name
				.Replace("Style", "")
				.ToLower()
				.GetEnum<ParagraphType>();
		}

		private static List<Piece> getPieces<T>(Paragraph<T> paragraph) where T : struct
		{
			return paragraph.Pieces
				.Where(p => !String.IsNullOrEmpty(p.Text))
				.ToList()
				.Select(PieceJson<T>.SetPiece)
				.ToList();
		}
	}
}
