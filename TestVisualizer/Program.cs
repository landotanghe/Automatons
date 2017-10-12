using FiniteAutomata.Visualizer;
using FiniteAutomota.NonDeterministic.Builder;

namespace TestVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var automaton = new AutomatonBuilder()
                .State("a").ActiveAtStart()
                .Transition().On('x').From("a").To("b")
                .State("b").Final()
                .Transition().On('y').From("b").To("c")
                .State("c")
                .Transition().On('z').From("a").To("c")
                .Build();
            var visualizer = new AutomatonVisualizer();
            visualizer.Visualize(automaton);
            //var painter = new ConsolePainter();
            //painter.DrawWarpedArrow(3, 3, 6, 16, 5, new char[]{'a',' ' }, false);
            //painter.DrawWarpedArrow(3, 6, 6, 13, 3, new char[] { 'b', ' ' }, true);
         //   painter.DrawWarpedArrow( 6, 13, 3, 6, -3, new char[] { 'b', ' ' });
            while (true)
            {
            }
        }
    }
}
