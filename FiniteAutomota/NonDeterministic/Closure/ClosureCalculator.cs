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
                var state = statesToAdd.Last();
                statesToAdd.Remove(state);

                closure.Add(state);
                FindNewCandidates(closure, statesToAdd, state);

            }

            return closure;
        }

        private static void FindNewCandidates<Descriptor, Symbol>(List<State<Descriptor, Symbol>> closure, List<State<Descriptor, Symbol>> statesToAdd, State<Descriptor, Symbol> state)
        {
            var epsilonNeighbours = state.GetEpsilonTransitions();
            foreach (var neighbour in epsilonNeighbours)
            {
                if (!closure.Contains(neighbour))
                {
                    statesToAdd.Add(neighbour);
                }
            }
        }
    }
}
