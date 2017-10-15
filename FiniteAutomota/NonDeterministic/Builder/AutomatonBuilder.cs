using FiniteAutomota.NonDeterministic.Builder.Exceptions;
using FiniteAutomota.NonDeterministic.Closure;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomota.NonDeterministic.Builder
{
    public partial class AutomatonBuilder<Descriptor, Symbol> : IAutomatonBuilder<Descriptor, Symbol>
    {
        private readonly IClosureCalculator _closureCalculator;

        private StateDefintionsManager<Descriptor, Symbol> StatesDefined = new StateDefintionsManager<Descriptor, Symbol>();
        private List<AddTransitionStep<Descriptor, Symbol>> TransitionsToAdd = new List<AddTransitionStep<Descriptor, Symbol>>();

        public AutomatonBuilder()
        {
            _closureCalculator = new ClosureCalculator();
        }

        internal AutomatonBuilder(IClosureCalculator closureCalculator)
        {
            _closureCalculator = closureCalculator;
        }

        public Automaton<Descriptor, Symbol> Build()
        {
            StatesDefined.Reset();
            var userDefinedStartStates = StatesDefined.UserDefinedStartStates();
            if (!userDefinedStartStates.Any())
            {
                throw new AtLeastOneStartStateRequiredException();
            }

            foreach (var transitions in TransitionsToAdd)
            {
                transitions.AddToSource(StatesDefined);
            }

            var finalSTates = StatesDefined.FinalStates();
            var automaton = new Automaton<Descriptor, Symbol>(userDefinedStartStates, finalSTates, _closureCalculator);

            return automaton;
        }
        
        public AddStateStep<Descriptor, Symbol> State(Descriptor description) {
            var existingStateDefinition = StatesDefined.FindStateDefinitionOrDefault(description);
            if(existingStateDefinition != null)
            {
                return existingStateDefinition;
            }

            var stateDefinition = new AddStateStep<Descriptor, Symbol>(description, this);
            StatesDefined.AddState(stateDefinition);
            return stateDefinition;
        }
        
        public AddTransitionStep<Descriptor, Symbol> Transition()
        {
            var transitionDefintion = new AddTransitionStep<Descriptor, Symbol>(this);
            TransitionsToAdd.Add(transitionDefintion);
            return transitionDefintion;
        }

        public IAutomatonBuilder<Descriptor, Symbol> SubSequence(IAutomatonBuilder<Descriptor, Symbol> subSequence, Descriptor description)
        {
            var subSequenceStep = new AddSubSequenceStep<Descriptor, Symbol>
            {
                SubSequenceBuilder = subSequence,
                Description = description
            };
            subSequenceStep.Reset();
            StatesDefined.AddSubsequence(subSequenceStep);
            return this;
        }
    }

    public class AutomatonBuilder : AutomatonBuilder<string, char>
    {
        public AutomatonBuilder() : base()
        {
        }

        public AutomatonBuilder(IClosureCalculator closureCalculator): base(closureCalculator)
        {
        }
    }
}
