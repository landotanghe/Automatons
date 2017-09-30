using System.Collections.Generic;

namespace FiniteAutomota.NonDeterministic.Closure
{
    public interface IClosureCalculator
    {
        List<State<Descriptor, Symbol>> GetClosureFor<Descriptor, Symbol>(IEnumerable<State<Descriptor, Symbol>> states);
    }
}
