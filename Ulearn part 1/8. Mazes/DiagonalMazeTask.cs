using System;

namespace Mazes
{
	public static class DiagonalMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
			if (width % 2 == 0)
				MoveIfWidthIsEven(robot, Math.Min(width, height) - 2);
			else
            {
				MoveIfHeightIsSix(robot, Math.Min(width, height) - 2, height);
				if (height % 2 == 0)
					MoveIfHeightIsEven(robot, Math.Min(width, height) - 2);
				else
					MoveIfHeightIsOdd(robot, Math.Min(width, height) - 2);
            }
		}
		
		public static void MoveIfHeightIsSix(Robot robot, int iterations, int height)
        {
			if (height == 6)
				for (int i = 0; i < iterations; i++)
				{
					MoveRight(robot, 3);
					MoveDown(robot, 1);
					if (robot.Finished)
						break;
				}
        }
		public static void MoveIfHeightIsOdd(Robot robot, int iterations)
        {
			for (int i = 0; i < iterations; i++)
            {
				MoveDown(robot, 2);
				if (i == iterations - 1)
					break;
				MoveRight(robot, 1);
            }
        }
		public static void MoveIfHeightIsEven(Robot robot, int iterations)
        {
			for (int i = 0; i < iterations; i++)
			{
				MoveDown(robot, 1);
				if (i == iterations - 1)
					break;
				MoveRight(robot, 1);
			}
		}
		public static void MoveIfWidthIsEven(Robot robot, int iterations)
        {
			for (int i = 0; i < iterations; i++)
			{
				MoveRight(robot, 3);
				if (i == iterations - 1)
					break;
				MoveDown(robot, 1);
			}
		}
		public static void MoveRight(Robot robot, int stepCount)
		{
			for (int i = 0; i < stepCount; i++)
				robot.MoveTo(Direction.Right);
		}

		public static void MoveDown(Robot robot, int stepCount)
		{
			for (int i = 0; i < stepCount; i++)
				robot.MoveTo(Direction.Down);
		}

		public static void MoveLeft(Robot robot, int stepCount)
		{
			for (int i = 0; i < stepCount; i++)
				robot.MoveTo(Direction.Left);
		}

	}
}