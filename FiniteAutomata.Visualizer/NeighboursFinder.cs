using FiniteAutomota.NonDeterministic;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomata.Visualizer
{
    public class NeighboursFinder
    {
        public List<Neighbour> GetNeighbours(State<string, char> state)
        {
            var symbols = state.GetKnownSymbols();
            var neighbours = new Dictionary<State<string,char>, Neighbour>();
            foreach(var symbol in symbols)
            {
                var transition = state.GetTransitionFor(symbol);
                foreach (var nextState in transition)
                {
                    if (!neighbours.ContainsKey(nextState))
                    {
                        neighbours.Add(nextState, new Neighbour(nextState));
                    }
                    neighbours[nextState].AddSymbol(symbol);
                }
            }

            var epsilonTransition = state.GetEpsilonTransitions();
            foreach (var nextState in epsilonTransition)
            {
                if (!neighbours.ContainsKey(nextState))
                {
                    neighbours.Add(nextState, new Neighbour(nextState));
                }
                neighbours[nextState].AddEpsilon();
            }

            return neighbours.Values.ToList();
        }
    }

    public class Neighbour
    {
        public State<string, char> State;
        public List<char> Symbols;
        private bool _isEpsilonIncluded;

        public Neighbour(State<string, char> state){
            State = state;
            Symbols = new List<char>();
            _isEpsilonIncluded = false;
        }
        
        public void AddSymbol(char symbol)
        {
            Symbols.Add(symbol);
        }

        public void AddEpsilon()
        {
            _isEpsilonIncluded = true;
        }

        public string Description => State.Description;

        public int Width => Description.Length + 4 + Symbols.Count + (_isEpsilonIncluded ? 1 : 0);
    }
}
