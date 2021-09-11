namespace Mazes
{
	public static class EmptyMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
			MoveRight(robot, width - 3);
			MoveDown(robot, height - 3);
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
	}
}