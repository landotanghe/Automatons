using FiniteAutomata.Visualizer;
using FiniteAutomota.NonDeterministic;
using FiniteAutomota.NonDeterministic.Builder;
using System;

namespace TestVisualizer
{
    class Program
    {
        private const string Subsequence1 = "Subsequence1";
        private const string Subsequence2 = "Subsequence2";
        private const string Start = "start";
        private const string Source1 = "source1";
        private const string Source2 = "source2";
        private const string Target1 = "target1";
        private const string Target2 = "target2";
        private const string Closure = "closure";
        private const char SymbolA = 'a';
        private const char SymbolB = 'b';

        static void Main(string[] args)
        {
            Console.BufferWidth = 2000;

            var subsequence = new AutomatonBuilder()
                .State("a").ActiveAtStart()
                .Transition().On('x').From("a").To("b")
                .State("b").Final();

            var Start = "start";
            var Target = "Target";
            var builder = new AutomatonBuilder()
                .State(Start).ActiveAtStart()
                .Transition().On('x').From(Start).To(Target)
                .State(Target).Final();

       //     var automaton1 = builder.Build();
        //    var automaton2 = builder.Build();


            var automaton = new AutomatonBuilder()
                .State("S").ActiveAtStart()
                .SubSequence(subsequence, "seq1")
                .SubSequence(subsequence, "seq3")
                .Transition().OnEpsilon().From("S").To("seq1")
                .Transition().On('x').From("seq1").To("X")
                .SubSequence(subsequence, "X")
                //.State("X")
                .Transition().OnEpsilon().From("X").To("seq3")
                .Transition().OnEpsilon().From("seq3").To("E")
                .State("E")
                .Build();



            var visualizer = new AutomatonVisualizer();
            visualizer.Visualize(Case1());
            //var painter = new ConsolePainter();
            //painter.DrawWarpedArrow(3, 3, 6, 16, 5, new char[]{'a',' ' }, false);
            //painter.DrawWarpedArrow(3, 6, 6, 13, 3, new char[] { 'b', ' ' }, true);
         //   painter.DrawWarpedArrow( 6, 13, 3, 6, -3, new char[] { 'b', ' ' });
            while (true)
            {
            }
        }

        private static Automaton<string, char> Case1()
        {

            var subSequence = new AutomatonBuilder()
                .State(Start).ActiveAtStart()
                .State(Target1).Final()
                .Transition().On('b').From(Start).To(Target1);

            return (new AutomatonBuilder<string,char>())
                .State(Start).ActiveAtStart()
                .State(Target1).Final()
                .SubSequence(subSequence, Subsequence1)
                .SubSequence(subSequence, Subsequence2)
                .Transition().On('a').From(Start).To(Subsequence1)
                .Transition().OnEpsilon().From(Subsequence1).To(Subsequence2)
                .Transition().On('c').From(Subsequence1).To(Target1).Build();
        }
    }
}
