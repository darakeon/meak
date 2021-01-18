using System;
using System.Collections.Generic;
using System.Web;
using GenderFix;
using Structure.Entities.System;

namespace Presentation.Helpers
{
	class AutomaticFix
	{
		internal Episode Story;
		internal IList<String> CharacterList;

		public static Boolean FixerReview =>
			HttpContext.Current.Request["FR"] == "1";

		internal void Fix()
		{
			var gender = new Gender();

			gender.AddRule("\\b([Dd])e MEAK", "$1a MEAK");
			gender.AddRule("\\bEm MEAK", "Na MEAK");
			gender.AddRule("\\bem MEAK", "na MEAK");

			foreach (var block in Story.BlockList)
			{
				foreach (var teller in block.TellerList)
				{
					foreach (var piece in teller.Pieces)
					{
						preFix(piece);
						piece.Text = gender.Remove(piece.Text);
					}
				}

				foreach (var talk in block.TalkList)
				{
					foreach (var piece in talk.Pieces)
					{
						var hasGenderFix = Story.HasGenderFix(piece.Style, talk.Character);

						if (hasGenderFix)
						{
							piece.Text = gender.Remove(piece.Text);
						}
					}
				}
			}
		}

		private void preFix<T>(Piece<T> piece)
			where T : struct
		{
			piece.Text = piece.Text
				.Replace(" num ", " em um ")
				.Replace(" numa ", " em uma ");
		}
	}
}
