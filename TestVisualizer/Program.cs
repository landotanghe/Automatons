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
        private const string Subsequence3 = "Subsequence3";
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
            
            var visualizer = new AutomatonVisualizer();
            visualizer.Visualize(Case_Repeated_AB());
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

            return (new AutomatonBuilder<string, char>())
                .State(Start).ActiveAtStart()
                .State(Target1).Final()
                .SubSequence(subSequence, Subsequence1)
                .SubSequence(subSequence, Subsequence2)
                .Transition().On('a').From(Start).To(Subsequence1)
                .Transition().OnEpsilon().From(Subsequence1).To(Subsequence2)
                .Transition().On('c').From(Subsequence1).To(Target1).Build();
        }


        private static Automaton<string, char> Case_Repeated_AB()
        {
            var subSequence = Repeated_AB();
            subSequence = Nest(subSequence);
            subSequence = Nest(subSequence);

            return subSequence.Build();
        }

        private static IAutomatonBuilder<string, char> Nest(IAutomatonBuilder<string, char> subSequence)
        {
            return new AutomatonBuilder<string, char>()
                            .State(Start).ActiveAtStart()
                            .State(Target1).Final()
                            .SubSequence(subSequence, Subsequence1)
                            .Transition().OnEpsilon().From(Start).To(Subsequence1)
                            .Transition().OnEpsilon().From(Subsequence1).To(Target1);
        }

        private static IAutomatonBuilder<string, char> Repeated_AB()
        {
            var subSequence = new AutomatonBuilder()
                .State(Start).ActiveAtStart()
                .State(Source1)
                .State(Target1).Final()
                .Transition().On('a').From(Start).To(Source1)
                .Transition().On('b').From(Source1).To(Target1);

            return new AutomatonBuilder<string, char>()
                .State(Start).ActiveAtStart()
                .State(Target1).Final()
                .SubSequence(subSequence, Subsequence1)
                .SubSequence(subSequence, Subsequence2)
                .SubSequence(subSequence, Subsequence3)
                .Transition().OnEpsilon().From(Start).To(Subsequence1)
                .Transition().OnEpsilon().From(Subsequence1).To(Subsequence2)
                .Transition().OnEpsilon().From(Subsequence2).To(Subsequence3)
                .Transition().OnEpsilon().From(Subsequence3).To(Target1);
        }

        private static Automaton<string, char> Case2()
        {

            var subSequence = new AutomatonBuilder()
                .State(Start).ActiveAtStart()
                .State(Target1).Final()
                .Transition().On('b').From(Start).To(Target1);

            return (new AutomatonBuilder<string, char>())
                .State(Start).ActiveAtStart()
                .State(Source1)
                .State(Source2)
                .State(Target1).Final()
                .State(Target2).Final()
                .SubSequence(subSequence, Subsequence1)
                .SubSequence(subSequence, Subsequence2)
                .Transition().On('a').From(Start).To(Subsequence1)
                .Transition().OnEpsilon().From(Subsequence1).To(Source1)
                .Transition().OnEpsilon().From(Source1).To(Source2)
                .Transition().OnEpsilon().From(Source2).To(Target1)
                .Transition().On('x').From(Subsequence1).To(Target2)
                .Transition().On('c').From(Target1).To(Target2)
                .Transition().On('d').From(Source1).To(Target2)
                .Transition().OnEpsilon().From(Target2).To(Target1)
                .Transition().OnEpsilon().From(Target2).To(Source1)
                .Build();
        }
    }
}
