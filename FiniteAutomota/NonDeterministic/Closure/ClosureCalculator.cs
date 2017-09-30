using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomota.NonDeterministic.Closure
{
    public class ClosureCalculator : IClosureCalculator
    {
        public List<State<Descriptor, Symbol>> GetClosureFor<Descriptor, Symbol>(IEnumerable<State<Descriptor, Symbol>> states)
        {
            var closure = states.ToList();
            foreach (var startState in states)
            {
                //TODO create a IClosureCalculator
                //TODO add unit tests for it and fix bugs
                closure.AddRange(startState.GetEpsilonTransitions());
            }
            return closure;
        }
    }
}
