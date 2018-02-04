using System;
using Structure.Entities.Json;
using Structure.Entities.System;
using Structure.Enums;
using Structure.Extensions;

namespace Structure.Data
{
	public class PieceJson<TS> where TS : struct
	{
		public static Piece<TS> GetPiece(Piece genericPiece, ParagraphType type)
		{
			return new Piece<TS>
			{
				Text = genericPiece.Text,
				Style = getStyle(genericPiece, type),
			};
		}

		private static TS getStyle(Piece genericPiece, ParagraphType type)
		{
			try
			{
				return genericPiece.Type.GetEnum<TS>();
			}
			catch
			{
				throw new Exception(
					$"Style {genericPiece.Type} not found in " +
					$"{type} (text: '{genericPiece.Text}')."
				);
			}
		}

		public static Piece SetPiece(Piece<TS> piece)
		{
			return new Piece
			{
				Text = piece.Text,
				Type = SetStyle(piece.Style),
			};
		}
		
		public static String SetStyle(TS style)
		{
			return style.ToString().ToLower();
		}
	}
}
