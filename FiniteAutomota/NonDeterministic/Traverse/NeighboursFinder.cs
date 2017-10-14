using FiniteAutomota.NonDeterministic;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomata.Visualizer
{
    public class NeighboursFinder<Descriptor, Symbol>
    {
        public List<Neighbour<Descriptor, Symbol>> GetNeighbours(State<Descriptor, Symbol> state)
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
                        neighbours.Add(nextState, new Neighbour<Descriptor, Symbol>(nextState));
                    }
                    neighbours[nextState].AddSymbol(symbol);
                }
            }

            var epsilonTransition = state.GetEpsilonTransitions();
            foreach (var nextState in epsilonTransition)
            {
                if (!neighbours.ContainsKey(nextState))
                {
                    neighbours.Add(nextState, new Neighbour<Descriptor, Symbol>(nextState));
                }
                neighbours[nextState].AddEpsilon();
            }

            return neighbours.Values.ToList();
        }
    }

    public class Neighbour<Descriptor, Symbol>
    {
        public State<Descriptor, Symbol> State;
        public List<Symbol> Symbols;
        public bool IsEpsilonIncluded { get; private set; }

        public Neighbour(State<Descriptor, Symbol> state){
            State = state;
            Symbols = new List<Symbol>();
            IsEpsilonIncluded = false;
        }
        
        public void AddSymbol(Symbol symbol)
        {
            Symbols.Add(symbol);
        }

        public void AddEpsilon()
        {
            IsEpsilonIncluded = true;
        }

        public Descriptor Description => State.Description;
    }
}
