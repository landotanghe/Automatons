using FiniteAutomota.NonDeterministic;
using System.Collections.Generic;

namespace FiniteAutomata.Visualizer
{
    public class AutomatonVisualizer
    {
        public Automaton Automaton { get; set; }

        public void Visualize()
        {
            List<StatesLine> lines = new List<StatesLine>();

            List<State<string, char>> VisualizedStates = new List<State<string, char>>();
            foreach(var startingState in Automaton.StartStates)
            {
                var line = new StatesLine();
                line.Add(startingState);
                lines.Add(line);
                VisualizedStates.Add(startingState);
            }

            foreach(var neighbour in Automaton.StartStates)
            {

            }
        }

        public class StatesLine
        {
            public List<State<string, char>> States = new List<State<string, char>>();

            public void Add(State<string, char> state)
            {
                States.Add(state);
            }

        }
    }
}
