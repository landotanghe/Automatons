using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomota.NonDeterministic.Closure
{
    public class ClosureCalculator : IClosureCalculator
    {
        public List<State<Descriptor, Symbol>> GetClosureFor<Descriptor, Symbol>(IEnumerable<State<Descriptor, Symbol>> states)
        {
            var closure = new List<State<Descriptor, Symbol>>();
            var statesToAdd = states.ToList();
            while (statesToAdd.Any())
            {
                State<Descriptor, Symbol> state = Pop(statesToAdd);
                
                if(!closure.Contains(state))
                    closure.Add(state);
                statesToAdd.AddRange(GetUnseenNeighbours(closure, state));
            }

            return closure;
        }

        private static State<Descriptor, Symbol> Pop<Descriptor, Symbol>(List<State<Descriptor, Symbol>> statesToAdd)
        {
            var state = statesToAdd.Last();
            statesToAdd.Remove(state);
            return state;
        }

        private static IEnumerable<State<Descriptor, Symbol>> GetUnseenNeighbours<Descriptor, Symbol>(List<State<Descriptor, Symbol>> closure, State<Descriptor, Symbol> state)
        {
            var epsilonNeighbours = state.GetEpsilonTransitions();
            return epsilonNeighbours.Where(neighbour => !closure.Contains(neighbour));
        }
    }
}
