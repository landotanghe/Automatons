using FiniteAutomota.NonDeterministic.Closure;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomota.NonDeterministic
{
    public class Automaton<Descriptor, Symbol>
    {
        private List<State<Descriptor, Symbol>> CurrentStates;
        private readonly List<State<Descriptor, Symbol>> _startStates;
        private readonly List<State<Descriptor, Symbol>> _acceptingStates;
        private readonly IClosureCalculator _closureCalculator;
        
        public Automaton(IEnumerable<State<Descriptor, Symbol>> startStates, IEnumerable<State<Descriptor, Symbol>> acceptingStates, IClosureCalculator closureCalculator)
        {
            _startStates = startStates.ToList();
            _acceptingStates = acceptingStates.ToList();
            Reset();

            _closureCalculator = closureCalculator;
        }
        
        internal List<State<Descriptor, Symbol>> StartStates => _startStates;
        internal List<State<Descriptor, Symbol>> FinalStates => _acceptingStates;

        public void Reset()
        {
            CurrentStates = _startStates.AsEnumerable().ToList();
        }

        internal IEnumerable<State<Descriptor, Symbol>> GetActiveStates()
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

        public bool IsAccepted()
        {
            return CurrentStates.Any(state => _acceptingStates.Contains(state));
        }
    }

    public class Automaton : Automaton<string, char>
    {
        public Automaton(IEnumerable<State<string, char>> startStates, IEnumerable<State<string, char>> acceptingStates, IClosureCalculator closureCalculator) : base(startStates, acceptingStates, closureCalculator)
        {
        }
    }
}
