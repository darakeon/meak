using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Translator
{
	internal class Verification
	{
		public Verification(String notAllowed)
		{
			this.notAllowed = notAllowed;
		}

		private String notAllowed { get; }

		public IList<string> NotAllowedMatches(String originalText)
		{
			return new Regex(notAllowed)
				.Matches(originalText)
				.SelectMany(m => m.Groups.Skip(1))
				.Select(g => g.ToString())
				.Distinct()
				.ToList();
		}
	}
}