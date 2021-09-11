namespace Pluralize
{
	public static class PluralizeTask
	{
		public static string PluralizeRubles(int count)
		{
			if (count == 1 || (count - 11) % 10 == 0 && count % 100 != 11)
				return "рубль";
			else if (count == 2 || count == 3 || count == 4)
				return "рубля";
			else if ( (count - 12) % 10 == 0 && count % 100 != 12 || (count - 13) % 10 == 0 
				&& count % 100 != 13 || (count - 14) % 10 == 0 && count % 100 != 14)
				return "рубля";
			else
				return "рублей";
		}
	}
}