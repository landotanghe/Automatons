using FiniteAutomata.Visualizer;

namespace TestVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var painter = new ConsolePainter();
            painter.DrawWarpedArrow(3, 3, 6, 16, 5, new char[]{'a',' ' }, false);
            painter.DrawWarpedArrow(3, 6, 6, 13, 3, new char[] { 'b', ' ' }, true);
         //   painter.DrawWarpedArrow( 6, 13, 3, 6, -3, new char[] { 'b', ' ' });
            while (true)
            {
            }
        }
    }
}
