namespace FiniteAutomata.Visualizer
{
    public interface IPainter
    {
        void DrawWarpedArrow(int row1, int col1, int row2, int col2, int depth);
        void DrawWarpedArrow(int row1, int col1, int row2, int col2, int depth, char[] symbols);
    }
}