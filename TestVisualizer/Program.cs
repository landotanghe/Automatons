using FiniteAutomata.Visualizer;

namespace TestVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var painter = new ConsolePainter();
            //painter.DrawWarpedArrow(2, 1, 5, 3, 1);
            painter.DrawWarpedArrow(3, 3, 6, 16, 5, new char[]{'a',' ' });
            painter.DrawWarpedArrow(3, 6, 6, 13, 3, new char[] { 'b', ' ' });
            while (true)
            {
            }
        }
    }
}
