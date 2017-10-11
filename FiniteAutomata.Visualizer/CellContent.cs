namespace FiniteAutomata.Visualizer
{
    public interface CellContent
    {
        int Height { get; }
        void Draw(IPainter painter);
    }
}
