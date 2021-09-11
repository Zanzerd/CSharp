using System;
using System.Collections.Generic;

namespace func_rocket
{
	public class LevelsTask
	{
		static readonly Physics standardPhysics = new Physics();

		public static IEnumerable<Level> CreateLevels()
		{
			yield return new Level("Zero", 
				new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
				new Vector(600, 200), 
				(size, v) => Vector.Zero, 
				standardPhysics);
			yield return new Level("Heavy",
				new Rocket(new Vector(500, 400), Vector.Zero, 0.5 * Math.PI),
				new Vector(50, 100),
				(size, v) => new Vector(0, 0.9), 
				standardPhysics);
			yield return new Level("Up",
				new Rocket(new Vector(350, 100), Vector.Zero, 0),
				new Vector(700, 500),
				(size, v) => {
					double d = size.Height - v.Y;
					return new Vector(0, -300 / (d + 300.0));
				},
				standardPhysics
				);
			yield return new Level("WhiteHole",
				new Rocket(new Vector(350, 100), Vector.Zero, 0),
				new Vector(700, 500),
				(size, v) =>
				{
					var target = new Vector(700, 500);
					var dir = target - v;
					double d = Math.Sqrt((v.X - target.X) * (v.X - target.X) + (v.Y - target.Y) * (v.Y - target.Y));
					double module = (140 * d) / (d * d + 1);
					return dir.Normalize() * (-module);
				},
				standardPhysics
				);
			yield return new Level("BlackHole",
				new Rocket(new Vector(350, 100), Vector.Zero, 0),
				new Vector(700, 500),
				(size, v) =>
                {
					var target = new Vector(700, 500);
					var source = new Vector(350, 100);
					var point = new Vector((target.X + source.X) / 2, (target.Y + source.Y) / 2);
					var dir = point - v;
					double d = Math.Sqrt((v.X - point.X) * (v.X - point.X) + (v.Y - point.Y) * (v.Y - point.Y));
					double module = (300 * d) / (d * d + 1);
					return dir.Normalize() * module;
				},
				standardPhysics
				);
			yield return new Level("BlackAndWhite",
				new Rocket(new Vector(350, 100), Vector.Zero, 0),
				new Vector(700, 500),
				(size, v) =>
				{
					var target = new Vector(700, 500);
					var source = new Vector(350, 100);
					var pointBlack = new Vector((target.X + source.X) / 2, (target.Y + source.Y) / 2);
					var dirWhite = target - v;
					double dWhite = Math.Sqrt((v.X - target.X) * (v.X - target.X) + (v.Y - target.Y) * (v.Y - target.Y));
					double moduleWhite = (140 * dWhite) / (dWhite * dWhite + 1);
					var dirBlack = pointBlack - v;
					double dBlack = Math.Sqrt((v.X - pointBlack.X) * (v.X - pointBlack.X) + 
						(v.Y - pointBlack.Y) * (v.Y - pointBlack.Y));
					double moduleBlack = (300 * dBlack) / (dBlack * dBlack + 1);
					var g1 = dirWhite.Normalize() * moduleWhite;
					var g2 = dirBlack.Normalize() * moduleBlack;
					var g = (dirBlack.Normalize() * moduleBlack + dirWhite.Normalize() * (-moduleWhite)) / 2;
					return g;
				},
				standardPhysics
				);
		}
	}
}