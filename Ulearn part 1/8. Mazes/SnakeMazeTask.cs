namespace Mazes
{
	public static class SnakeMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
			for (int i = 0; i < height / 2; i++)
			{
				MoveRight(robot, width - 3);
				MoveDown(robot, 2);
				MoveLeft(robot, width - 3);
				if (i == (height / 4 - 1))
					break;
				else
					MoveDown(robot, 2);
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