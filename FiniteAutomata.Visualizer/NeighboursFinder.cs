using FiniteAutomota.NonDeterministic;
using System.Collections.Generic;

namespace FiniteAutomata.Visualizer
{
    public class NeighboursFinder<Descriptor, Symbol>
    {
        public IEnumerable<Neighbour<Descriptor, Symbol>> GetNeighbours(State<Descriptor, Symbol> state)
        {
            var symbols = state.GetKnownSymbols();
            var neighbours = new Dictionary<State<Descriptor, Symbol>, Neighbour<Descriptor, Symbol>>();
            foreach(var symbol in symbols)
            {
                var transition = state.GetTransitionFor(symbol);
                foreach (var nextState in transition)
                {
                    if (!neighbours.ContainsKey(nextState))
                    {
                        neighbours.Add(nextState, new Neighbour<Descriptor, Symbol> (nextState));
                    }
                    neighbours[nextState].AddSymbol(symbol);
                }
            }
            return neighbours.Values;
        }
    }

    public class Neighbour<Descriptor, Symbol>
    {
        public Neighbour(State<Descriptor, Symbol> state){
            _state = state;
            _symbols = new List<Symbol>();
        }

        private State<Descriptor, Symbol> _state { get; set; }
        private List<Symbol> _symbols { get; set; }

        public void AddSymbol(Symbol symbol)
        {
            _symbols.Add(symbol);
        }
    }
}
