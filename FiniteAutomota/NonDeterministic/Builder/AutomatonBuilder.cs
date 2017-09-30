using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomota.NonDeterministic.Builder
{
    public partial class AutomatonBuilder<Descriptor, Symbol> : IAutomatonBuilder<Descriptor, Symbol>
    {
        private StateDefintionsManager<Descriptor, Symbol> StatesDefined = new StateDefintionsManager<Descriptor, Symbol>();
        private List<AddTransitionStep<Descriptor, Symbol>> TransitionsToAdd = new List<AddTransitionStep<Descriptor, Symbol>>();
        
        public Automaton<Descriptor, Symbol> Build()
        {
            foreach(var transitionToAdd in TransitionsToAdd)
            {
                var source = StatesDefined.FindState(transitionToAdd.Source);
                var target = StatesDefined.FindState(transitionToAdd.Target);

                source.AddEpsilonTransition(target);
            }

            var startStates = StatesDefined.UserDefinedStartStates();
            var closure = startStates.ToList();
            foreach (var startState in startStates)
            {
                //TODO create a IClosureCalculator
                //TODO add unit tests for it and fix bugs
                closure.AddRange(startState.GetEpsilonTransitions());
            }
            startStates = closure;

            var automaton = new Automaton<Descriptor, Symbol>(startStates);
            return automaton;
        }
        
        public AddStateStep<Descriptor, Symbol> State(Descriptor description) {
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
    }
    
    public class AutomatonBuilder : AutomatonBuilder<string, char>
    {
    }
}
