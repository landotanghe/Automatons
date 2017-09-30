using System.Collections.Generic;

namespace FiniteAutomota.NonDeterministic
{
    public class Automaton<Descriptor, Symbol>
    {
        private readonly List<State<Descriptor, Symbol>> CurrentStates;
        public Automaton(State<Descriptor, Symbol> startState)
        {
            CurrentStates = new List<State<Descriptor, Symbol>>();
            CurrentStates.Add(startState);
        }

        public Automaton(IEnumerable<State<Descriptor, Symbol>> startStates)
        {
            CurrentStates = new List<State<Descriptor, Symbol>>();
            CurrentStates.AddRange(startStates);
        }

        public IEnumerable<State<Descriptor, Symbol>> GetActiveStates()
        {
            return CurrentStates;
        }

        public void Process(Symbol symbol)
        {
        }
    }

    public class Automaton : Automaton<string, char>
    {
        public Automaton(State<string, char> startState) : base(startState)
        {
        }
    }
}
