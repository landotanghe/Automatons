using System.Collections.Generic;

namespace FiniteAutomota.NonDeterministic
{
    public class State<Descriptor, Symbol>
    {
        public Descriptor Description { get; private set; }
        private List<State<Descriptor, Symbol>> _epsilonTransitions = new List<State<Descriptor, Symbol>>();
        private Dictionary<Symbol, List<State<Descriptor, Symbol>>> _transitions = new Dictionary<Symbol, List<State<Descriptor, Symbol>>>();

        internal State(Descriptor description)
        {
            Description = description;
        }
        
        internal void AddEpsilonTransition(State<Descriptor, Symbol> target)
        {
            _epsilonTransitions.Add(target);
        }

        internal void AddTransition(Symbol symbol, State<Descriptor, Symbol> target)
        {
            if (! _transitions.ContainsKey(symbol))
            {
                _transitions.Add(symbol, new List<State<Descriptor, Symbol>>());
            }

            _transitions[symbol].Add(target);
        }

        internal IEnumerable<State<Descriptor, Symbol>> GetEpsilonTransitions()
        {
            return _epsilonTransitions;
        }

        public override string ToString()
        {
            return Description.ToString();
        }

        internal List<State<Descriptor, Symbol>> GetTransitionsFor(Symbol symbol)
        {
            if (! _transitions.ContainsKey(symbol))
            {
                return new List<State<Descriptor, Symbol>>();
            }

            return _transitions[symbol];
        }
    }

    public class State : State<string, char>
    {
        public State(string description) : base(description)
        {
        }
    }
}
