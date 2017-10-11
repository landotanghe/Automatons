namespace FiniteAutomata.Visualizer
{
    public interface IPainter
    {
        void DrawRightWardArrow(int row, int col1, int col2, int depth);
        void DrawRightWardArrow(int row, int col1, int col2, int depth, char[] symbols);
    }
}