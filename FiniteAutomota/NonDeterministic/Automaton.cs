using FiniteAutomota.NonDeterministic.Closure;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomota.NonDeterministic
{
    public class Automaton<Descriptor, Symbol>
    {
        public Descriptor Description { get; }
        private List<State<Descriptor, Symbol>> CurrentStates;
        private List<State<Descriptor, Symbol>> StartStates;
        private readonly IClosureCalculator _closureCalculator;
        
        public Automaton(IEnumerable<State<Descriptor, Symbol>> startStates, IClosureCalculator closureCalculator, Descriptor description)
        {
            StartStates = startStates.AsEnumerable().ToList();
            Reset();

            _closureCalculator = closureCalculator;
            Description = description;
        }
        
        public void Reset()
        {
            CurrentStates = StartStates.AsEnumerable().ToList();
        }

        public IEnumerable<State<Descriptor, Symbol>> GetActiveStates()
        {
            return CurrentStates;
        }

        public void Process(Symbol symbol)
        {
            var nextStates = CurrentStates
                .Select(state => state.GetTransitionsFor(symbol))
                .SelectMany(state => state)
                .Distinct();

            CurrentStates = _closureCalculator.GetClosureFor(nextStates);
        }
    }

    public class Automaton : Automaton<string, char>
    {
        public Automaton(IEnumerable<State<string, char>> startStates, IClosureCalculator closureCalculator, string description) : base(startStates, closureCalculator, description)
        {
        }
    }
}
