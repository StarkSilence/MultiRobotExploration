namespace MultiRobotExploration.Model
{
    public static class CellExtension
    {
        public static bool Is(this Cell cell, Cell other)
        {
            return (cell & other) == other;
        }
    }
}