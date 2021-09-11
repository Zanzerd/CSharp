using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace linq_slideviews
{
	public class ParsingTask
	{
		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
		/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
		/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			var linesSplit = lines
				.Skip(1)
				.Select(line => Regex.Split(line, @"[;]"));
			var linesFiltered = linesSplit
				.Where(arr =>  arr.Length == 3 && int.TryParse(arr[0], out int n) 
				&& Enum.TryParse<SlideType>(arr[1], true, out _));
			var dictResult = linesFiltered
				.ToDictionary(arr => int.Parse(arr[0]), arr => new SlideRecord(int.Parse(arr[0]),
					(SlideType)Enum.Parse(typeof(SlideType), arr[1], true), arr[2]));
			return dictResult;
		}

		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
		/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
		/// Такой словарь можно получить методом ParseSlideRecords</param>
		/// <returns>Список информации о посещениях</returns>
		/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
		public static IEnumerable<VisitRecord> ParseVisitRecords(
			IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
		{
			var linesSplit = lines
				.Skip(1)
				//.Where(line => !string.IsNullOrEmpty(line))
				.ToDictionary(line => Regex.Split(line, @"[;]"), line => line);
				//.Select(line => Regex.Split(line, @"[.;,\s+]"));
			
			var linesFiltered = linesSplit
				.Select(kvp =>
				{
					if (kvp.Key.Length == 4 && !kvp.Key.Any(w => string.IsNullOrEmpty(w)))
					{
						var joined2and3 = string.Join(" ", kvp.Key[2], kvp.Key[3]);
						int slideId;
						if (int.TryParse(kvp.Key[0], out int _) && int.TryParse(kvp.Key[1], out int _)
						&& DateTime.TryParse(joined2and3, out _))
						{
							slideId = int.Parse(kvp.Key[1]);
							if (slides.ContainsKey(slideId))
								return new VisitRecord(int.Parse(kvp.Key[0]), slideId,
								DateTime.Parse(joined2and3), slides[slideId].SlideType);
						}
					}
					throw new FormatException($"Wrong line [{kvp.Value}]");
				});
			return linesFiltered;
		}
	}
}
