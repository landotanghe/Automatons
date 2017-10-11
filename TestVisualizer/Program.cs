using FiniteAutomata.Visualizer;

namespace TestVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var painter = new ConsolePainter();
            painter.DrawRightWardArrow(2, 1, 5, 3);
            painter.DrawRightWardArrow(3, 2, 6, 3, new char[]{'a',' ' });
            while (true)
            {
            }
        }
    }
}
