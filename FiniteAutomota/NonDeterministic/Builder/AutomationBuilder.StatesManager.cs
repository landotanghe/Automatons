using FiniteAutomota.NonDeterministic.Builder.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomota.NonDeterministic.Builder
{
    public partial class AutomatonBuilder<Descriptor, Symbol> : IAutomatonBuilder<Descriptor, Symbol>
    {
		public class StateDefintionsManager<Descriptor, Symbol>
        {
            private List<AddStateStep<Descriptor, Symbol>> StatesToAdd = new List<AddStateStep<Descriptor, Symbol>>();

            public void AddState(AddStateStep<Descriptor, Symbol> state)
            {
                StatesToAdd.Add(state);
            }
			
            private bool Equals(Descriptor description1, Descriptor description2)
            {
                return EqualityComparer<Descriptor>.Default.Equals(description1, description2);
            }

			public State<Descriptor, Symbol> FindState(Descriptor description)
            {
                var result = StatesToAdd
                    .Select(state => state.StateToBuild)
                    .Where(state => Equals(state.Description, description))
					.SingleOrDefault();

				if(result == null)
                    throw new UndefinedStateException(description);
                return result;
            }

			public List<State<Descriptor, Symbol>> UserDefinedStartStates()
            {
                return StatesToAdd
					.Where(state => state.IsActiveAtStart)
                    .Select(state => state.StateToBuild)
					.ToList();
            }
        }
    }
}