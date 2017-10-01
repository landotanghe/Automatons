using System.Collections.Generic;

namespace FiniteAutomota.NonDeterministic
{
    public class State<Descriptor, Symbol>
    {
        public Descriptor Description { get; private set; }
        internal List<State<Descriptor, Symbol>> _epsilonTransitions = new List<State<Descriptor, Symbol>>();

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
        }

        internal IEnumerable<State<Descriptor, Symbol>> GetEpsilonTransitions()
        {
            return _epsilonTransitions;
        }
    }

    public class State : State<string, char>
    {
        public State(string description) : base(description)
        {
        }
    }
}
